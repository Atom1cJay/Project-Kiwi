using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizGroundBoost : AMove
{
    MovementInputInfo mii;
    MovementInfo mi;

    public HorizGroundBoost(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
            mi.currentSpeedHoriz,
            movementSettings.GroundBoostMaxSpeedX,
            movementSettings.GroundBoostSensitivityX,
            movementSettings.GroundBoostGravityX);
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override float GetRotationThisFrame()
    {
        return movementSettings.GroundBoostRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        // todo change
        return new Run(mm, mii, mi, movementSettings);
    }

    public override string asString()
    {
        return "horizgroundboost";
    }
}
