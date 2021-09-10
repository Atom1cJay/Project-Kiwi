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
    Vector2 xVel;
    float yVel;

    public Knockback(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector3 normal, float strength) : base(ms, mi, mii)
    {
        timeElapsedRecovery = 0;
        recovering = false;
        xVel = new Vector2(normal.x, normal.z).normalized * strength * Mathf.Cos(movementSettings.KnockbackYAngle);
        yVel = strength * Mathf.Sin(movementSettings.KnockbackYAngle);
    }

    public override void AdvanceTime()
    {
        if (recovering)
        {
            timeElapsedRecovery += Time.deltaTime;
        }
        else
        {
            yVel -= movementSettings.KnockbackYGravity * Time.deltaTime;
        }

        if (mi.TouchingGround())
        {
            recovering = true;
            xVel = Vector2.zero;
            yVel = 0;
        }
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return xVel;
    }

    public override float GetVertSpeedThisFrame()
    {
        return yVel;
    }

    public override IMove GetNextMove()
    {
        if (timeElapsedRecovery > movementSettings.KnockbackRecoveryTime)
        {
            return new Idle(mii, mi, movementSettings);
        }
        return this;
    }

    public override float GetRotationSpeed()
    {
        return 0;
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
