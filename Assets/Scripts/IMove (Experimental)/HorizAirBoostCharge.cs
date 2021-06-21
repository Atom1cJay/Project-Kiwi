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
    MovementSettings ms;
    MovementInputInfo mii;
    MovementInfo mi;

    public HorizAirBoostCharge(MovementMaster mm, MovementSettings ms, MovementInputInfo mii, MovementInfo mi, float prevVertVel) : base(mm)
    {
        vertVel = (prevVertVel < 0) ? 0 : prevVertVel;

        timeCharging = 0;
        maxTimeToCharge = mm.airBoostMaxChargeTime;

        // To detect end of boost
        ia = mm.ia();

        this.ms = ms;
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
            mi.currentSpeed, 0, 0, ms.horizBoostChargeGravityX);
    }

    public override float GetVertSpeedThisFrame()
    {
        float gravityType = (vertVel > 0) ? ms.defaultGravity : ms.horizBoostChargeGravity;
        vertVel -= gravityType * Time.deltaTime;
        return vertVel;
    }

    public override IMove GetNextMove()
    {
        timeCharging += Time.deltaTime;

        if (timeCharging > maxTimeToCharge || ia.Player.Boost.ReadValue<float>() == 0)
        {
            return new HorizAirBoost(mm, ms, mii, mi, (timeCharging / maxTimeToCharge) * mm.airBoostMaxTime);
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
