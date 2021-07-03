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

    public abstract Vector2 GetHorizSpeedThisFrame();

    public abstract float GetVertSpeedThisFrame();

    public abstract float GetRotationSpeed();

    public abstract IMove GetNextMove();

    public abstract bool IncrementsTJcounter();

    public abstract bool TJshouldBreak();

    public abstract bool AdjustToSlope();

    public abstract string AsString();

    /// <summary>
    /// Gives the vector of horizontal movement that the player should move,
    /// given that they are moving forward relative to their rotation, and given
    /// the horizontal speed.
    /// </summary>
    protected Vector2 ForwardMovement(float horizSpeed)
    {
        Transform transform = mi.GetPlayerTransform();
        float horizAngleFaced = Mathf.Atan2(transform.forward.z, transform.forward.x);
        float xDelta = horizSpeed * Mathf.Cos(horizAngleFaced);
        float zDelta = horizSpeed * Mathf.Sin(horizAngleFaced);
        return new Vector2(xDelta, zDelta);
    }
}
