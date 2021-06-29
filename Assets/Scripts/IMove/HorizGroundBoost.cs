using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizGroundBoost : AMove
{
    float horizVel;
    readonly MovementInputInfo mii;
    readonly MovementInfo mi;

    public HorizGroundBoost(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        this.mii = mii;
        this.mi = mi;
    }

    public override void AdvanceTime()
    {
        horizVel = InputUtils.SmoothedInput(
            mi.currentSpeedHoriz,
            movementSettings.GroundBoostMaxSpeedX,
            movementSettings.GroundBoostSensitivityX,
            movementSettings.GroundBoostGravityX);
    }

    public override float GetHorizSpeedThisFrame()
    {
        return horizVel;
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override float GetRotationSpeed()
    {
        return movementSettings.GroundBoostRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        // todo change
        return new Run(mm, mii, mi, movementSettings);
    }

    public override string AsString()
    {
        return "horizgroundboost";
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return false;
    }
}
