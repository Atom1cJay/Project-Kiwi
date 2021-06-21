using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizGroundBoost : AMove
{
    MovementSettings ms;
    MovementInputInfo mii;
    MovementInfo mi;

    public HorizGroundBoost(MovementMaster mm, MovementSettings ms, MovementInputInfo mii, MovementInfo mi) : base(mm)
    {
        this.ms = ms;
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
            mi.currentSpeed,
            ms.groundBoostMaxSpeedX,
            ms.groundBoostSensitivityX,
            ms.groundBoostGravityX);
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        // todo change
        return new Run(mm, ms, mii, mi);
    }

    public override string asString()
    {
        return "horizgroundboost";
    }
}
