using System;
using UnityEngine;
using System.Collections;

public class HorizAirBoost : AMove
{
    float vertVel;

    public HorizAirBoost(HorizontalMovement hm, VerticalMovement vm, MovementMaster mm) : base(hm, vm, mm)
    {
        vertVel = 0;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return hm.airBoostSpeed;
    }

    public override float GetVertSpeedThisFrame()
    {
        vertVel -= vm.airBoostGravity * Time.fixedDeltaTime;
        return vertVel;
    }

    public override IMove GetNextMove()
    {
        throw new NotImplementedException();
    }


    public override string ToString()
    {
        return "horizairboost";
    }
}
