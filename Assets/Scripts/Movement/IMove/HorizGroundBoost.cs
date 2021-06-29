using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizGroundBoost : AMove
{
    float horizVel;

    /// <summary>
    /// Constructs a HorizGroundBoost, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public HorizGroundBoost(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;
    }

    public override void AdvanceTime()
    {
        horizVel = InputUtils.SmoothedInput(
            horizVel,
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
        return new Run(mii, mi, movementSettings, horizVel);
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

    public override bool AdjustToSlope()
    {
        return true;
    }
}
