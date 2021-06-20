using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO THIS MOVE CURRENTLY DOESNT WORK
public class HorizGroundBoost : AMove
{
    public HorizGroundBoost(HorizontalMovement hm, VerticalMovement vm, MovementMaster mm) : base(hm, vm, mm) { }

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
            hm.currentSpeed,
            hm.groundBoostSpeed,
            hm.groundBoostSensitivity,
            hm.groundBoostGravity);
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return "horizgroundboost";
    }
}
