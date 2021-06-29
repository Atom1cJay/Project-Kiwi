using UnityEngine;
using System.Collections;

public abstract class AMove : IMove
{
    protected readonly MovementMaster mm;
    protected readonly MovementSettingsSO movementSettings;

    protected AMove(MovementMaster mm, MovementSettingsSO movementSettings)
    {
        this.mm = Utilities.RequireNonNull(mm);
        this.movementSettings = Utilities.RequireNonNull(movementSettings);
    }

    public abstract void AdvanceTime();

    public abstract float GetHorizSpeedThisFrame();

    public abstract float GetVertSpeedThisFrame();

    public abstract float GetRotationSpeed();

    public abstract IMove GetNextMove();

    public abstract bool IncrementsTJcounter();

    public abstract bool TJshouldBreak();

    public abstract string AsString();
}
