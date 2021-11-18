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
    private bool receivedBasicHit;
    private Vector3 basicHitNormal;

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
        mii.OnPushPress.AddListener(() => horizBoostChargePending = true);
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mii.OnGlide.AddListener(() => glidePending = true);
        mii.OnJump.AddListener(() => jumpPending = true);
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
        mi.ph.onBasicHit.AddListener((Vector3 basicHitNormal) => { receivedBasicHit = true; this.basicHitNormal = basicHitNormal; });
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
        float startingMagn = Math.Min(horizVector.magnitude, mi.GetEffectiveSpeed().magnitude);
        horizVector = horizVector.normalized * startingMagn;
        bool inReverse = (horizVector + mii.GetRelativeHorizontalInputToCamera()).magnitude < horizVector.magnitude;
        // Choose which type of sensitivity to employ
        if (horizVector.magnitude < movementSettings.MaxSpeed)
        {
            horizVector += inReverse ?
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpSensitivityReverseX * Time.deltaTime
                :
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpSensitivityX * Time.deltaTime;
        }
        else if (horizVector.magnitude >= movementSettings.MaxSpeed)
        {
            float magn = horizVector.magnitude;
            horizVector += inReverse ?
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpSensitivityReverseX * Time.deltaTime
                :
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpAdjustSensitivityX * Time.deltaTime;
            horizVector = horizVector.normalized * (magn - (movementSettings.JumpSpeedDecRateOverMaxSpeed * Time.deltaTime));
        }
        // Don't let above the magnitude limit
        /*
        if (!mii.PressingBoost() && horizVector.magnitude > movementSettings.MaxSpeed)
        {
            horizVector = horizVector.normalized * movementSettings.MaxSpeed;
        }
        if (mii.PressingBoost() && horizVector.magnitude > movementSettings.GroundBoostMaxSpeedX)
        {
            horizVector = horizVector.normalized * movementSettings.GroundBoostMaxSpeedX;
        }
        */
        // Come to a stop
        if (mii.GetRelativeHorizontalInputToCamera().magnitude < 0.1f)
        {
            float magn = horizVector.magnitude;
            magn -= movementSettings.JumpGravityX * Time.deltaTime;
            horizVector = horizVector.normalized * magn;
        }
        // Limit Speed
        if (horizVector.magnitude > movementSettings.MaxSpeedAbsolute)
        {
            horizVector = horizVector.normalized * movementSettings.MaxSpeedAbsolute;
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
        if (receivedBasicHit)
        {
            return new Knockback(mii, mi, movementSettings, basicHitNormal, horizVector);
        }
        if (glidePending)
        {
            return new Glidev3(mii, mi, movementSettings, horizVector);
        }
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        if (PlayerSlopeHandler.ShouldSlide)
        {
            return new Slide(mii, mi, movementSettings, horizVector);
        }
        if (mi.TouchingGround() && !PlayerSlopeHandler.ShouldSlide && horizVector.magnitude > 0)
        {
            return new Run(mii, mi, movementSettings, horizVector, FromStatus.FromAir);
        }
        if (mi.TouchingGround() && !PlayerSlopeHandler.ShouldSlide && horizVector.magnitude == 0)
        {
            return new Idle(mii, mi, movementSettings);
        }
        if (jumpPending && coyoteTime > 0)
        {
            if (horizVector.magnitude == 0)
            {
                return new Idle(mii, mi, movementSettings, FromStatus.FromAir);
            }
            return new Run(mii, mi, movementSettings, horizVector, FromStatus.FromAir);
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

    public override Attack GetAttack()
    {
        return movementSettings.JumpAttack;
    }
}
