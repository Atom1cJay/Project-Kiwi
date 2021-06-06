using UnityEngine;
using System.Collections;

public abstract class AMove : IMove
{
    protected HorizontalMovement hm;

    protected AMove(HorizontalMovement hm)
    {
        if (hm == null)
        {
            Debug.LogError("Null HorizontalMovement passed into dive");
        }
        this.hm = hm;
    }

    public abstract float GetHorizSpeedThisFrame();
}
