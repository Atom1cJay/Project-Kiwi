using System;
using UnityEngine;

/// <summary>
/// Represents movement on the ground while jumping is not occurring.
/// </summary>
public class Run : AMove
{
    MovementSettings ms;
    MovementInputInfo mii;
    MovementInfo mi;

    public Run(MovementMaster mm, MovementSettings ms, MovementInputInfo mii, MovementInfo mi) : base(mm)
    {
        this.ms = ms;
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        if (mi.currentSpeed > ms.defaultMaxSpeedX)
        {
            return
                InputUtils.SmoothedInput(
                    mi.currentSpeed,
                    mii.GetHorizontalInput().magnitude * ms.defaultMaxSpeedX,
                    ms.runSensitivityX,
                    ms.runGravityX);
        }
        else
        {
            return
                InputUtils.SmoothedInput(
                    mi.currentSpeed,
                    mii.GetHorizontalInput().magnitude * ms.defaultMaxSpeedX,
                    ms.runSensitivityX,
                    ms.runGravityX);
        }
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        if (mm.IsJumping() && mm.tripleJumpValid())
        {
            return new TripleJump(mm, ms, mii, mi);
        }
        if (mm.IsJumping())
        {
            return new Jump(mm, ms, mii, mi);
        }
        if (!mm.IsOnGround())
        {
            return new Fall(mm, ms, mii, mi);
        }
        if (mm.IsInHardTurn())
        {
            return new HardTurn(mm, ms, mii, mi);
        }
        // todo make ground boost possible

        return this;
    }

    public override string asString()
    {
        return "run";
    }
}
