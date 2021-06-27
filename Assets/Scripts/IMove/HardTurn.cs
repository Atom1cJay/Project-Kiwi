using System;
using UnityEngine;

public class HardTurn : AMove
{
    float timeLeft;
    bool tookJumpInput;
    MovementInputInfo mii;
    MovementInfo mi;

    public HardTurn(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        mm.mm_OnJump.AddListener(onJumpInput);
        timeLeft = mm.hardTurnTime;
        this.mii = mii;
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

    private void onJumpInput()
    {
        tookJumpInput = true;
    }

    public override IMove GetNextMove()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            return new Run(mm, mii, mi, movementSettings);
        }
        if (tookJumpInput)
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
