using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertAirBoostCharge : AMove
{
    float vertVel;
    float timeActive;
    float maxTimeActive;
    InputActions ia;
    MovementSettings ms;
    MovementInputInfo mii;
    MovementInfo mi;

    public VertAirBoostCharge(MovementMaster mm, MovementSettings ms, MovementInputInfo mii, MovementInfo mi, float prevVertVel) : base(mm)
    {
        vertVel = (prevVertVel < 0) ? 0 : prevVertVel;
        timeActive = 0;
        maxTimeActive = mm.vertAirBoostMaxChargeTime;
        ia = mm.ia();
        this.ms = ms;
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
            mi.currentSpeed, 0, 0, ms.vertBoostChargeGravityX);
    }

    public override float GetVertSpeedThisFrame()
    {
        float gravityType = (vertVel > 0) ? ms.defaultGravity : ms.horizBoostChargeGravity;
        vertVel -= gravityType * Time.fixedDeltaTime;
        return vertVel;
    }

    public override IMove GetNextMove()
    {
        timeActive += Time.fixedDeltaTime;

        if (timeActive > maxTimeActive || ia.Player.VertBoost.ReadValue<float>() == 0)
        {
            float propCharged = Mathf.Clamp01(timeActive / maxTimeActive);
            return new VertAirBoost(mm, ms, mii, mi, propCharged);
        }

        return this;
    }

    public override string asString()
    {
        return "vertairboostcharge";
    }
}
