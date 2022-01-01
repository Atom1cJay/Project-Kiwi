using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the dive move in the air
/// </summary>
public class Knockback : AMove
{
    bool recovering;
    float timeElapsedRecovery;
    readonly Vector2 initHorizVel;
    Vector2 horizVel;
    float timePassed = 0;
    float yVel;

    public Knockback(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector3 normal, Vector2 horizVectorHeadingIn) : base(ms, mi, mii)
    {
        timeElapsedRecovery = 0;
        recovering = false;
        yVel = ms.KnockbackInitYVel;
        // Normal for horizontal knockback (gotten from knockback data)
        Vector2 horizNormal = new Vector2(normal.x, normal.z);
        // If the horiz. knockback would be zero, use the opposite of the vector coming into this move
        if (horizNormal.normalized == Vector2.zero)
        {
            // If this would cause zero horiz. knockback, either choose randomly or do the opposite of incoming vector
            if (horizVectorHeadingIn.normalized == Vector2.zero)
            {
                horizNormal = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
            }
            else
            {
                horizNormal = -horizVectorHeadingIn.normalized;
            }
        }
        horizVel = ms.KnockbackInitXVel * horizNormal.normalized;
        initHorizVel = horizVel;
    }

    public override void AdvanceTime()
    {
        horizVel = Vector2.Lerp(initHorizVel, Vector2.zero, timePassed / movementSettings.KnockbackXTimeToZero);
        if (recovering)
        {
            timeElapsedRecovery += Time.deltaTime;
        }
        else
        {
            yVel -= movementSettings.KnockbackYGravity * Time.deltaTime;
        }

        if (mi.TouchingGround() && yVel < 0)
        {
            recovering = true;
            horizVel = Vector2.zero;
            yVel = 0;
        }

        timePassed += Time.deltaTime;
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return horizVel;
    }

    public override float GetVertSpeedThisFrame()
    {
        return yVel;
    }

    public override IMove GetNextMove()
    {
        // Handle Feedback Moves
        IMove feedbackMove = GetFeedbackMove(horizVel);
        if (feedbackMove != null)
        {
            return feedbackMove;
        }
        // Handle Everything Else
        if (timeElapsedRecovery > movementSettings.KnockbackRecoveryTime)
        {
            return new Idle(mii, mi, movementSettings);
        }
        return this;
    }

    public override RotationInfo GetRotationInfo()
    {
        return new RotationInfo(0, false);
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return true;
    }

    public override bool AdjustToSlope()
    {
        return false;
    }

    public override string AsString()
    {
        return "knockback";
    }

}
