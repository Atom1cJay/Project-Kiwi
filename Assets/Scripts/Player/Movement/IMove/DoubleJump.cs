using System;
using System.Collections;
using UnityEngine;

public class DoubleJump : AJump
{
    /// <summary>
    /// Constructs a DoubleJump, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public DoubleJump(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(mii, mi, ms) { }

    public override void AdvanceTime()
    {
        base.AdvanceTimeVertical(
            movementSettings.JumpCancelledGravityIncrease,
            movementSettings.JumpUncancelledGravityIncrease,
            movementSettings.JumpMaxUncancelledGravity,
            movementSettings.JumpMaxCancelledGravity,
            movementSettings.JumpMinVel);
        base.AdvanceTimeHorizontal(
            movementSettings.JumpSensitivityReverseX,
            movementSettings.JumpSensitivityX,
            movementSettings.JumpAdjustSensitivityX,
            movementSettings.JumpSpeedDecRateOverMaxSpeed,
            movementSettings.JumpGravityX);
    }

    public override string AsString()
    {
        return "doublejump";
    }
}
