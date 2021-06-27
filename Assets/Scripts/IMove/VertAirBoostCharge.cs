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
    MovementInputInfo mii;
    MovementInfo mi;

    public VertAirBoostCharge(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, float prevVertVel) : base(mm)
    {
        vertVel = (prevVertVel < 0) ? 0 : prevVertVel;
        timeActive = 0;
        maxTimeActive = mm.vertAirBoostMaxChargeTime;
        ia = mm.ia();
        this.mii = mii;
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

    public override IMove GetNextMove()
    {
        timeActive += Time.fixedDeltaTime;

        if (timeActive > maxTimeActive || ia.Player.VertBoost.ReadValue<float>() == 0)
        {
            float propCharged = Mathf.Clamp01(timeActive / maxTimeActive);
            return new VertAirBoost(mm, mii, mi, propCharged);
        }

        return this;
    }

    public override string asString()
    {
        return "vertairboostcharge";
    }
}
