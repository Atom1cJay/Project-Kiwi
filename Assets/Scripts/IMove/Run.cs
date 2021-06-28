using System;
using UnityEngine;

/// <summary>
/// Represents movement on the ground while jumping is not occurring.
/// </summary>
public class Run : AMove
{
    MovementInputInfo mii;
    MovementInfo mi;
    bool jumpPending;

    public Run(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        this.mii = mii;
        mii.OnJump.AddListener(() => jumpPending = true);
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

    public override float GetRotationThisFrame()
    {
        return movementSettings.GroundRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (mi.currentSpeedHoriz == 0)
        {
            return new Idle(mm, mii, mi, movementSettings);
        }
        if (jumpPending && mm.tripleJumpValid())
        {
            return new TripleJump(mm, mii, mi, movementSettings);
        }
        if (jumpPending)
        {
            return new Jump(mm, mii, mi, movementSettings);
        }
        if (!mi.touchingGround())
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
