using System;
using UnityEngine;

public class HardTurn : AMove
{
    public HardTurn(HorizontalMovement hm) : base(hm) { }

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
                   hm.currentSpeed, 0, 0, hm.hardTurnGravity);
    }
}
