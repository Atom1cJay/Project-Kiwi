using System;
using UnityEngine;

/// <summary>
/// Represents movement on the ground while jumping is not occurring.
/// </summary>
public class Run : AMove
{
    MovementInputInfo mii;
    MovementInfo mi;

    public Run(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        if (mi.currentSpeedHoriz > movementSettings.MaxSpeed)
        {
            return
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.RunSensitivityX,
                    movementSettings.RunGravityX);
        }
        else
        {
            return
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.RunSensitivityX,
                    movementSettings.RunGravityX);
        }
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        if (mi.currentSpeedHoriz == 0)
        {
            return new Idle(mm, mii, mi, movementSettings);
        }
        if (mm.IsJumping() && mm.tripleJumpValid())
        {
            return new TripleJump(mm, mii, mi, movementSettings);
        }
        if (mm.IsJumping())
        {
            return new Jump(mm, mii, mi, movementSettings);
        }
        if (!mm.IsOnGround())
        {
            return new Fall(mm, mii, mi, movementSettings);
        }
        if (mm.IsInHardTurn())
        {
            return new HardTurn(mm, mii, mi, movementSettings);
        }
        // todo make ground boost possible

        return this;
    }

    public override string asString()
    {
        return "run";
    }
}
