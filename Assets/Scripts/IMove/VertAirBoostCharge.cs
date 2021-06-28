using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertAirBoostCharge : AMove
{
    float vertVel;
    float timeActive;
    float maxTimeActive;
    MovementInputInfo mii;
    MovementInfo mi;
    bool boostReleasePending;

    public VertAirBoostCharge(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, float prevVertVel, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = (prevVertVel < 0) ? 0 : prevVertVel;
        timeActive = 0;
        maxTimeActive = movementSettings.VertBoostMaxChargeTime;
        this.mii = mii;
        mii.OnVertBoostRelease.AddListener(() => boostReleasePending = true);
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
            mi.currentSpeedHoriz, 0, 0, movementSettings.VertBoostChargeGravityX);
    }

    public override float GetVertSpeedThisFrame()
    {
        float gravityType = (vertVel > 0) ? movementSettings.DefaultGravity : movementSettings.HorizBoostChargeGravity;
        vertVel -= gravityType * Time.fixedDeltaTime;
        return vertVel;
    }

    public override float GetRotationThisFrame()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        timeActive += Time.fixedDeltaTime;

        if (timeActive > maxTimeActive || boostReleasePending)
        {
            float propCharged = Mathf.Clamp01(timeActive / maxTimeActive);
            return new VertAirBoost(mm, mii, mi, propCharged, movementSettings);
        }

        return this;
    }

    public override string asString()
    {
        return "vertairboostcharge";
    }
}
