using UnityEngine;
using System.Collections;

public abstract class AMove : IMove
{
    protected readonly MovementSettingsSO movementSettings;
    protected readonly MovementInfo mi;
    protected readonly MovementInputInfo mii;
    // Bools representing causes for "feedback" moves,
    // moves which should take place, no matter what the current move is,
    // if the player encounters something.
    private bool receivedJumpFeedback;
    private bool receivedKnockbackFeedback;
    private Vector3 knockbackFeedbackNormal; // Only to be accessed if received knockback feedback

    protected AMove(MovementSettingsSO movementSettings, MovementInfo mi,
        MovementInputInfo mii)
    {
        this.movementSettings = Utilities.RequireNonNull(movementSettings);
        this.mi = Utilities.RequireNonNull(mi);
        this.mii = Utilities.RequireNonNull(mii);
        // Feedback Move Events
        mi.onJumpAttackFeedbackReceived.AddListener(() => receivedJumpFeedback = true);
        mi.ph.onBasicHit.AddListener((Vector3 basicHitNormal) => { receivedKnockbackFeedback = true; knockbackFeedbackNormal = basicHitNormal; });
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

    public virtual Attack GetAttack() { return null; }

    public virtual MovementParticleInfo.MovementParticles GetParticlesToSpawn() { return null; }

    /// <summary>
    /// Gives the vector of horizontal movement that the player should move,
    /// given that they are moving forward relative to their rotation, and given
    /// the horizontal speed.
    /// </summary>
    protected Vector2 ForwardMovement(float speed)
    {
        Transform transform = mi.GetPlayerTransform();
        float horizAngleFaced = Mathf.Atan2(transform.forward.z, transform.forward.x);
        float xDelta = speed * Mathf.Cos(horizAngleFaced);
        float zDelta = speed * Mathf.Sin(horizAngleFaced);
        return new Vector2(xDelta, zDelta);
    }

    /// <summary>
    /// Gives the vector of horizontal movement that the player should move,
    /// given that they are moving sideways relative to their rotation, and given
    /// the horizontal speed.
    /// </summary>
    protected Vector2 SideMovement(float speed)
    {
        Transform transform = mi.GetPlayerTransform();
        float horizAngleFaced = Mathf.Atan2(transform.forward.z, transform.forward.x) + (Mathf.PI / 2);
        float xDelta = speed * Mathf.Cos(horizAngleFaced);
        float zDelta = speed * Mathf.Sin(horizAngleFaced);
        return new Vector2(xDelta, zDelta);
    }

    /// <summary>
    /// Given a vector of horizontal movement, gives the magnitude shared
    /// between that vector and the angle faced by the player. Useful
    /// for converting Vector2-based horizontal movement into float-based.
    /// </summary>
    /// <param name="horizVector"></param>
    /// <returns></returns>
    protected float GetSharedMagnitudeWithPlayerAngle(Vector2 horizVector)
    {
        float playerAngle = mi.GetPlayerTransform().eulerAngles.y * Mathf.Deg2Rad;
        float vectorAngle = (-Mathf.Atan2(horizVector.y, horizVector.x)) + (Mathf.PI / 2);
        float angleDifference = playerAngle - vectorAngle;
        return Mathf.Cos(angleDifference) * horizVector.magnitude;
    }

    public enum FromStatus
    {
        Null,
        FromIdle,
        FromAir,
    }

    /// <summary>
    /// Gives the feedback move that should be transitioned to this frame,
    /// if there is one.
    /// </summary>
    /// <returns></returns>
    protected IMove GetFeedbackMove(Vector2 horizVector)
    {
        if (receivedJumpFeedback)
        {
            return new Jump(mii, mi, movementSettings, horizVector.magnitude);
        }
        if (mi.TouchingWater())
        {
            return new Swim(mii, mi, movementSettings, horizVector);
        }
        if (receivedKnockbackFeedback)
        {
            return new Knockback(mii, mi, movementSettings, knockbackFeedbackNormal, horizVector);
        }
        return null;
    }
}
