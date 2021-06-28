using UnityEngine;
using System.Collections;

public abstract class AMove : IMove
{
    protected MovementMaster mm;
    protected MovementSettingsSO movementSettings;

    protected AMove(MovementMaster mm, MovementSettingsSO movementSettings)
    {
        if (mm == null)
        {
            Debug.LogError("Null args passed into move");
        }
        this.mm = mm;
        this.movementSettings = movementSettings;
    }

    public abstract float GetHorizSpeedThisFrame();

    public abstract float GetVertSpeedThisFrame();

    public abstract float GetRotationThisFrame();

    public abstract IMove GetNextMove();

    public abstract string asString();
}
