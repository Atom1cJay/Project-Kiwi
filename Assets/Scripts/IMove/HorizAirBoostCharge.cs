using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizAirBoostCharge : AMove
{
    float vertVel;
    float timeCharging;
    float maxTimeToCharge;
    MovementInputInfo mii;
    MovementInfo mi;
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

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
            mi.currentSpeedHoriz, 0, 0, movementSettings.HorizBoostChargeGravityX);
    }

    public override float GetVertSpeedThisFrame()
    {
        float gravityType = (vertVel > 0) ? movementSettings.DefaultGravity : movementSettings.HorizBoostChargeGravity;
        vertVel -= gravityType * Time.deltaTime;
        return vertVel;
    }

    public override float GetRotationThisFrame()
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

    public override string asString()
    {
        return "horizairboost";
    }
}
