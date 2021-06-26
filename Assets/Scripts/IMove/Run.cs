using System;
using UnityEngine;

/// <summary>
/// Represents movement on the ground while jumping is not occurring.
/// </summary>
public class Run : AMove
{
    MovementInputInfo mii;
    MovementInfo mi;

    public Run(MovementMaster mm, MovementInputInfo mii, MovementInfo mi) : base(mm)
    {
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        if (mi.currentSpeed > movementSettings.MaxSpeed)
        {
            return
                InputUtils.SmoothedInput(
                    mi.currentSpeed,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.RunSensitivityX,
                    movementSettings.RunGravityX);
        }
        else
        {
            return
                InputUtils.SmoothedInput(
                    mi.currentSpeed,
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
        if (mi.currentSpeed == 0)
        {
            return new Idle(mm, mii, mi);
        }
        if (mm.IsJumping() && mm.tripleJumpValid())
        {
            return new TripleJump(mm, mii, mi);
        }
        if (mm.IsJumping())
        {
            return new Jump(mm, mii, mi);
        }
        if (!mm.IsOnGround())
        {
            return new Fall(mm, mii, mi);
        }
        if (mm.IsInHardTurn())
        {
            return new HardTurn(mm, mii, mi);
        }
        // todo make ground boost possible

        return this;
    }

    public override string asString()
    {
        return "run";
    }
}
