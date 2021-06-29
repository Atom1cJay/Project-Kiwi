using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizAirBoostCharge : AMove
{
    float horizVel;
    float vertVel;
    float timeCharging;
    readonly float maxTimeToCharge;
    readonly MovementInputInfo mii;
    readonly MovementInfo mi;
    bool boostReleasePending;

    public HorizAirBoostCharge(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, float prevVertVel, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = (prevVertVel < 0) ? 0 : prevVertVel;

        timeCharging = 0;
        maxTimeToCharge = movementSettings.HorizBoostMaxChargeTime;

        this.mii = mii;
        mii.OnHorizBoostRelease.AddListener(() => boostReleasePending = true);
        this.mi = mi;
    }

    public override void AdvanceTime()
    {
        // Horizontal
        horizVel = InputUtils.SmoothedInput(
            mi.currentSpeedHoriz, 0, 0, movementSettings.HorizBoostChargeGravityX);
        float gravityType = (vertVel > 0) ? movementSettings.DefaultGravity : movementSettings.HorizBoostChargeGravity;
        // Vertical
        vertVel -= gravityType * Time.deltaTime;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return horizVel;
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
        timeCharging += Time.deltaTime;

        if (timeCharging > maxTimeToCharge || boostReleasePending)
        {
            return new HorizAirBoost(mm, mii, mi, (timeCharging / maxTimeToCharge) * movementSettings.HorizBoostMaxTime, movementSettings);
        }
        else
        {
            return this;
        }
    }

    public override string AsString()
    {
        return "horizairboost";
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return true;
    }
}
