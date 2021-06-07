using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizAirBoostCharge : AMove
{
    float vertVel;

    public HorizAirBoostCharge(HorizontalMovement hm, VerticalMovement vm, MovementMaster mm, float prevVertVel) : base(hm, vm, mm)
    {
        vertVel = (prevVertVel < 0) ? 0 : prevVertVel;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
            hm.currentSpeed, 0, 0, hm.airBoostChargeGravity);
    }

    public override float GetVertSpeedThisFrame()
    {
        float gravityType = (vertVel > 0) ? vm.nonJumpGravity : vm.airBoostChargeGravity;
        vertVel -= gravityType * Time.fixedDeltaTime;
        return vertVel;
    }

    public override IMove GetNextMove()
    {
        throw new NotImplementedException();
    }
}
