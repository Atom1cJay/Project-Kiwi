using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : AMove
{
    Vector2 horizVector;
    float vertVel;
    bool divePending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;
    bool jumpPending;
    bool groundPoundPending;
    bool glidePending;
    bool swimPending;
    float coyoteTime;

    /// <summary>
    /// Constructs a Fall, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public Fall(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector, bool giveCoyoteTime) : base(ms, mi, mii)
    {
        this.horizVector = horizVector;
        vertVel = 0;
        coyoteTime = giveCoyoteTime ? movementSettings.CoyoteTime : 0;
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", AllowCoyoteTime());
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnVertBoostCharge.AddListener(() => vertBoostChargePending = true);
        mii.OnHorizBoostCharge.AddListener(() => horizBoostChargePending = true);
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mii.OnGlide.AddListener(() => glidePending = true);
        mii.OnJump.AddListener(() => jumpPending = true);
        mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
    }

    IEnumerator AllowCoyoteTime()
    {
        while (coyoteTime > 0)
        {
            coyoteTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public override void AdvanceTime()
    {
        // Horizontal
        //float startingMagn = Math.Min(horizVector.magnitude, mi.GetEffectiveSpeed());
        //horizVector = horizVector.normalized * startingMagn;
        // Choose which type of sensitivity to employ
        if (horizVector.magnitude < movementSettings.MaxSpeed)
        {
            horizVector += mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpSensitivityX * Time.deltaTime;
        }
        else if (horizVector.magnitude >= movementSettings.MaxSpeed)
        {
            horizVector += mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpAdjustSensitivityX * Time.deltaTime;
        }
        // Don't let above the magnitude limit
        if (!mii.PressingBoost() && horizVector.magnitude > movementSettings.MaxSpeed)
        {
            horizVector = horizVector.normalized * movementSettings.MaxSpeed;
        }
        if (mii.PressingBoost() && horizVector.magnitude > movementSettings.GroundBoostMaxSpeedX)
        {
            horizVector = horizVector.normalized * movementSettings.GroundBoostMaxSpeedX;
        }
        // Come to a stop
        if (mii.GetRelativeHorizontalInputToCamera().magnitude < 0.1f)
        {
            float magn = horizVector.magnitude;
            magn -= movementSettings.JumpGravityX * Time.deltaTime;
            horizVector = horizVector.normalized * magn;
        }
        // Vertical
        vertVel -= movementSettings.DefaultGravity * Time.deltaTime;
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return horizVector;
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override float GetRotationSpeed()
    {
        if (divePending || horizBoostChargePending || vertBoostChargePending)
        {
            return float.MaxValue;
        }
        return movementSettings.AirRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, horizVector);
        }
        if (PlayerSlopeHandler.BeyondMaxAngle && mi.TouchingGround())
        {
            return new Slide(mii, mi, movementSettings, horizVector);
        }
        if (glidePending)
        {
            return new Glidev3(mii, mi, movementSettings, horizVector);
        }
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        if (mi.TouchingGround() && horizVector.magnitude > 0)
        {
            return new Run(mii, mi, movementSettings, horizVector);
        }
        if (mi.TouchingGround() && horizVector.magnitude == 0)
        {
            return new Idle(mii, mi, movementSettings);
        }
        if (jumpPending && coyoteTime > 0)
        {
            return new Jump(mii, mi, movementSettings, horizVector.magnitude);
        }
        if (divePending)
        {
            return new Dive(mii, mi, movementSettings);
        }
        if (horizBoostChargePending && (!mi.InAntiBoostZone() || vertVel > 0))
        {
            return new HorizAirBoostCharge(mii, mi, movementSettings, vertVel, horizVector);
        }
        if (vertBoostChargePending && (!mi.InAntiBoostZone() || vertVel > 0))
        {
            return new VertAirBoostCharge(mii, mi, movementSettings, vertVel, horizVector);
        }

        return this;
    }

    public override string AsString()
    {
        return "fall";
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return true;
    }

    public override bool AdjustToSlope()
    {
        return true;
    }
}
