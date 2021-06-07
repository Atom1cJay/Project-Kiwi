using UnityEngine;
using System.Collections;

public abstract class AMove : IMove
{
    protected HorizontalMovement hm;
    protected VerticalMovement vm;
    protected MovementMaster mm;

    protected AMove(HorizontalMovement hm, VerticalMovement vm, MovementMaster mm)
    {
        if (hm == null || vm == null)
        {
            Debug.LogError("Null Horiz/VertMovement passed into dive");
        }
        this.hm = hm;
        this.vm = vm;
        this.mm = mm;
    }

    public abstract float GetHorizSpeedThisFrame();

    public abstract float GetVertSpeedThisFrame();

    public abstract IMove GetNextMove();
}
