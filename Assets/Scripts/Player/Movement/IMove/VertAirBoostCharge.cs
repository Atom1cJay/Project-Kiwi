using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertAirBoostCharge : AMove
{
    float vertVel;
    float horizVel;
    float timeActive;
    readonly float maxTimeActive;
    bool boostReleasePending;
    bool swimPending;

    /// <summary>
    /// Constructs a VertAirBoostCharge, initializing the objects that hold all
    /// the information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    public VertAirBoostCharge(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float vertVel, Vector2 horizVector) : base(ms, mi, mii)
    {
        horizVel = GetSharedMagnitudeWithPlayerAngle(horizVector);
        this.vertVel = (vertVel < 0) ? 0 : vertVel;
        timeActive = 0;
        maxTimeActive = movementSettings.VertBoostMaxChargeTime;
        mii.OnVertBoostRelease.AddListener(() => boostReleasePending = true);
        mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
    }

    public override void AdvanceTime()
    {
        // Meta
        timeActive += Time.deltaTime;
        // Vertical
        float gravityType = (vertVel > 0) ?
            movementSettings.DefaultGravity : movementSettings.HorizBoostChargeGravity;
        vertVel -= gravityType * Time.fixedDeltaTime;
        // Horizontal
        horizVel = InputUtils.SmoothedInput(
            horizVel, 0, 0, movementSettings.VertBoostChargeGravityX);
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
        return 0;
    }

    public override IMove GetNextMove()
    {
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (timeActive > maxTimeActive || boostReleasePending)
        {
            float propCharged = Mathf.Clamp01(timeActive / maxTimeActive);
            return new VertAirBoost(mii, mi, propCharged, movementSettings, horizVel);
        }
        return this;
    }

    public override string AsString()
    {
        return "vertairboostcharge";
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
        return false;
    }
}
