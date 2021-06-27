using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizAirBoostCharge : AMove
{
    float vertVel;
    float timeCharging;
    float maxTimeToCharge;
    InputActions ia;
    MovementInputInfo mii;
    MovementInfo mi;

    public HorizAirBoostCharge(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, float prevVertVel, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = (prevVertVel < 0) ? 0 : prevVertVel;

        timeCharging = 0;
        maxTimeToCharge = mm.airBoostMaxChargeTime;

        // To detect end of boost
        ia = mm.ia();

        this.mii = mii;
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

    public override IMove GetNextMove()
    {
        timeCharging += Time.deltaTime;

        if (timeCharging > maxTimeToCharge || ia.Player.Boost.ReadValue<float>() == 0)
        {
            return new HorizAirBoost(mm, mii, mi, (timeCharging / maxTimeToCharge) * mm.airBoostMaxTime, movementSettings);
        }
        else
        {
            return this;
        }
    }

    public override string asString()
    {
        return "horizairboostcharge";
    }
}
