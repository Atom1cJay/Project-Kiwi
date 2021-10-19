using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizGroundBoostCharge : AMove
{
    float horizVel;
    float timeCharging;
    readonly float maxTimeToCharge;
    bool boostReleasePending;
    bool swimPending;

    /// <summary>
    /// Constructs a HorizAirBoostCharge, initializing the objects that hold all
    /// the information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="vertVel">The vertical speed moving into this move</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public HorizGroundBoostCharge(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(ms, mi, mii)
    {
        horizVel = GetSharedMagnitudeWithPlayerAngle(horizVector);
        timeCharging = 0;
        maxTimeToCharge = movementSettings.HorizBoostMaxChargeTime;
        mii.OnHorizBoostRelease.AddListener(() => boostReleasePending = true);
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
    }

    public override void AdvanceTime()
    {
        timeCharging += Time.deltaTime;
        // Horizontal
        horizVel = InputUtils.SmoothedInput(
            horizVel, 0, 0, movementSettings.HorizBoostChargeGravityXGround);
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return ForwardMovement(horizVel);
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override float GetRotationSpeed()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (timeCharging > maxTimeToCharge || boostReleasePending)
        {
            float propCharged = timeCharging / maxTimeToCharge;
            float wantedVel = movementSettings.HorizBoostMinSpeedGroundX + (propCharged * (movementSettings.HorizBoostMaxSpeedGroundX - movementSettings.HorizBoostMinSpeedGroundX));
            return new BoostSlide(mii, mi, movementSettings, wantedVel, true);
        }
        else
        {
            return this;
        }
    }

    public override string AsString()
    {
        return "horizgroundboostcharge";
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
