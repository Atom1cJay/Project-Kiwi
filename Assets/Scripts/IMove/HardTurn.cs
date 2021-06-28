using System;
using UnityEngine;

public class HardTurn : AMove
{
    float timeLeft;
    bool jumpInputPending;
    MovementInputInfo mii;
    MovementInfo mi;

    public HardTurn(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        timeLeft = movementSettings.HardTurnTime;
        this.mii = mii;
        mii.OnJump.AddListener(() => jumpInputPending = true);
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
                   mi.currentSpeedHoriz, 0, 0, movementSettings.HardTurnGravityX);
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override float GetRotationThisFrame()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            return new Run(mm, mii, mi, movementSettings);
        }
        if (jumpInputPending)
        {
            return new Jump(mm, mii, mi, movementSettings);
        }
        return this;
    }

    public override string asString()
    {
        return "hardturn";
    }
}
