using UnityEngine;
using System.Collections;

public abstract class AMove : IMove
{
    protected readonly MovementSettingsSO movementSettings;
    protected readonly MovementInfo mi;
    protected readonly MovementInputInfo mii;
    protected static float pushMaintainTimeLeft { get; private set; }

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

    public virtual bool RotationIsRelative()
    {
        return false;
    }

    public virtual float CameraRotateTowardsRatio()
    {
        return 0;
    }

    public virtual float CameraVerticalAutoTarget()
    {
        return 0;
    }

    public virtual Attack GetAttack()
    {
        return null;
    }

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

    // Starts the timer for push maintain time. The way this timer's state
    // is handled depends on the move.
    protected void StartPushMaintainTime()
    {
        pushMaintainTimeLeft = movementSettings.PushSpeedMaintainTime;
        MonobehaviourUtils.Instance.EndCoroutine(WaitForPushMaintainEnd());
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitForPushMaintainEnd());
    }

    // Starts the timer for push maintain time. The way this timer's state
    // is handled depends on the move.
    protected void EndPushMaintainTime()
    {
        pushMaintainTimeLeft = 0;
    }

    IEnumerator WaitForPushMaintainEnd()
    {
        while (pushMaintainTimeLeft > 0)
        {
            pushMaintainTimeLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        pushMaintainTimeLeft = 0;
    }
}
