using System;
using UnityEngine;

public class HardTurn : AMove
{
    float timeLeft;
    bool tookJumpInput;

    public HardTurn(HorizontalMovement hm, VerticalMovement vm, MovementMaster mm) : base(hm, vm, mm)
    {
        mm.mm_OnJump.AddListener(onJumpInput);
        timeLeft = mm.hardTurnTime;
    }

    public override float GetHorizSpeedThisFrame()
    {
        timeLeft -= Time.fixedDeltaTime; // Needs to happen each frame
        return InputUtils.SmoothedInput(
                   hm.currentSpeed, 0, 0, hm.hardTurnGravity);
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
        if (timeLeft < 0)
        {
            return new Run(hm, vm, mm);
        }
        if (tookJumpInput)
        {
            return new Jump(hm, vm, mm);
        }
        return this;
    }


    public override string ToString()
    {
        return "hardturn";
    }
}
