using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : AMove
{
    float horizVel;
    float vertVel;
    bool divePending;
    bool vertBoostPending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;
    bool jumpPending;
    bool groundPoundPending;
    float coyoteTime;
    bool glidePending;

    /// <summary>
    /// Constructs a Fall, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public Fall(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel, bool giveCoyoteTime) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;
        vertVel = 0;
        coyoteTime = giveCoyoteTime ? movementSettings.CoyoteTime : 0;
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", AllowCoyoteTime());
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnVertBoostRelease.AddListener(() => vertBoostPending = true);
        mii.OnVertBoostCharge.AddListener(() => vertBoostChargePending = true);
        mii.OnHorizBoostCharge.AddListener(() => horizBoostChargePending = true);
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mii.OnJump.AddListener(() => jumpPending = true);
        mii.OnGlide.AddListener(() => glidePending = true);
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
        horizVel = Math.Min(horizVel, mi.GetEffectiveSpeed());

        if (mii.AirReverseInput())
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    movementSettings.GroundBoostMaxSpeedX,
                    movementSettings.GroundBoostSensitivityX,
                    movementSettings.GroundBoostGravityX);
        }
        else if (horizVel < 0)
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirReverseSensitivityX,
                    movementSettings.AirReverseGravityX);
            if (horizVel < 0) horizVel = 0;
        }
        else if (horizVel > movementSettings.MaxSpeed)
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirSensitivityX,
                    movementSettings.AirGravityXOverTopSpeed);
        }
        else
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirSensitivityX,
                    movementSettings.AirGravityX);
        }
        // Vertical
        vertVel -= movementSettings.DefaultGravity * Time.deltaTime;
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return ForwardMovement(horizVel);
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override float GetRotationSpeed()
    {
        if (mii.PressingBoost())
        {
            return movementSettings.GroundBoostRotationSpeed;
        }
        if (mii.AirReverseInput())
        {
            return 0;
        }
        return movementSettings.AirRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (glidePending)
        {
            return new Glidev3(mii, mi, movementSettings, horizVel);
        }
        if (vertBoostPending)
        {
            return new VertAirBoost(mii, mi, mii.VertBoostTimeCharged() / movementSettings.VertBoostMaxChargeTime, movementSettings, horizVel);
        }
        if (PlayerSlopeHandler.BeyondMaxAngle && mi.TouchingGround())
        {
            return new Slide(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        if (mi.TouchingGround() && horizVel > 0)
        {
            return new Run(mii, mi, movementSettings, horizVel);
        }
        if (mi.TouchingGround() && horizVel == 0)
        {
            return new Idle(mii, mi, movementSettings);
        }
        if (jumpPending && coyoteTime > 0)
        {
            return new Jump(mii, mi, movementSettings, horizVel);
        }
        if (divePending)
        {
            return new Dive(mii, mi, movementSettings);
        }
        if (horizBoostChargePending)
        {
            return new HorizAirBoostCharge(mii, mi, movementSettings, vertVel, horizVel);
        }
        if (vertBoostChargePending)
        {
            return new VertAirBoostCharge(mii, mi, movementSettings, vertVel, horizVel);
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
