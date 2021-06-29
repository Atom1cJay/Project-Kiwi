using UnityEngine;
using System.Collections;

public abstract class AMove : IMove
{
    protected readonly MovementSettingsSO movementSettings;
    protected readonly MovementInfo mi;
    protected readonly MovementInputInfo mii;

    protected AMove(MovementSettingsSO movementSettings, MovementInfo mi,
        MovementInputInfo mii)
    {
        this.movementSettings = Utilities.RequireNonNull(movementSettings);
        this.mi = Utilities.RequireNonNull(mi);
        this.mii = Utilities.RequireNonNull(mii);
    }

    public abstract void AdvanceTime();

    public abstract float GetHorizSpeedThisFrame();

    public abstract float GetVertSpeedThisFrame();

    public abstract float GetRotationSpeed();

    public abstract IMove GetNextMove();

    public abstract bool IncrementsTJcounter();

    public abstract bool TJshouldBreak();

    public abstract bool AdjustToSlope();

    public abstract string AsString();
}
