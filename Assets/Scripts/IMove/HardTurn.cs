using System;
using UnityEngine;

public class HardTurn : AMove
{
    float timeLeft;
    bool tookJumpInput;
    MovementSettings ms;
    MovementInputInfo mii;
    MovementInfo mi;

    public HardTurn(MovementMaster mm, MovementSettings ms, MovementInputInfo mii, MovementInfo mi) : base(mm)
    {
        mm.mm_OnJump.AddListener(onJumpInput);
        timeLeft = mm.hardTurnTime;
        this.ms = ms;
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
                   mi.currentSpeed, 0, 0, ms.hardTurnGravityX);
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
            return new Run(mm, ms, mii, mi);
        }
        if (tookJumpInput)
        {
            return new Jump(mm, ms, mii, mi);
        }
        return this;
    }

    public override string asString()
    {
        return "hardturn";
    }
}
