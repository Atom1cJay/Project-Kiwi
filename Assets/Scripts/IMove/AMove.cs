using UnityEngine;
using System.Collections;

public abstract class AMove : IMove
{
    protected MovementMaster mm;
    protected MovementSettingsSO movementSettings = MovementSettingsSO.instance;

    protected AMove(MovementMaster mm)
    {
        if (mm == null)
        {
            Debug.LogError("Null args passed into move");
        }
        this.mm = mm;
    }

    public abstract float GetHorizSpeedThisFrame();

    public abstract float GetVertSpeedThisFrame();

    public abstract IMove GetNextMove();

    public abstract string asString();
}
