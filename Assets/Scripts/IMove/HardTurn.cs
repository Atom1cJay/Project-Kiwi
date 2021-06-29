using System;
using UnityEngine;

public class HardTurn : AMove
{
    float horizVel;
    float timeLeft;
    bool jumpInputPending;
    readonly MovementInputInfo mii;
    readonly MovementInfo mi;

    public HardTurn(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        timeLeft = movementSettings.HardTurnTime;
        this.mii = mii;
        mii.OnJump.AddListener(() => jumpInputPending = true);
        this.mi = mi;
    }

    public override void AdvanceTime()
    {
        // Meta
        timeLeft -= Time.deltaTime;
        // Horizontal
        horizVel = InputUtils.SmoothedInput(
            mi.currentSpeedHoriz, 0, 0, movementSettings.HardTurnGravityX);
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
        return 0;
    }

    public override IMove GetNextMove()
    {
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

    public override string AsString()
    {
        return "hardturn";
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return true;
    }
}
