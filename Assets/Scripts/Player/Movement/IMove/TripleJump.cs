using System;
using UnityEngine;
using System.Collections;

public class TripleJump : AJump
{
    /// <summary>
    /// Constructs a TripleJump, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public TripleJump(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(mii, mi, ms, ms.TjInitJumpVel, ms.TjVelocityMultiplier) { }

    public override void AdvanceTime()
    {
        base.AdvanceTimeVertical(
            movementSettings.TjGravityIncRateAtCancel,
            movementSettings.TjGravityIncRate,
            movementSettings.TjUncancelledMaxGravity,
            movementSettings.TjCancelledMaxGravity,
            -Mathf.Infinity);
        base.AdvanceTimeHorizontal(
            movementSettings.JumpSensitivityX,
            movementSettings.JumpSensitivityX,
            movementSettings.JumpAdjustSensitivityX,
            movementSettings.JumpSpeedDecRateOverMaxSpeed,
            movementSettings.JumpGravityX);
    }

    public override string AsString()
    {
        return "triplejump";
    }
}
