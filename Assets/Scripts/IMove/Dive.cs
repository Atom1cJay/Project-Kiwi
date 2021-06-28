using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the dive move in the air
/// </summary>
public class Dive : AMove
{
    float vertVel;
    MovementInputInfo mii;
    MovementInfo mi;

    public Dive(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = movementSettings.DiveInitVel;
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return movementSettings.DiveSpeedX;
    }

    public override float GetVertSpeedThisFrame()
    {
        vertVel -= movementSettings.DiveGravity * Time.deltaTime;
        return vertVel;
    }

    public override float GetRotationThisFrame()
    {
        return movementSettings.DiveRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (mi.touchingGround())
        {
            return new Run(mm, mii, mi, movementSettings);
        }
        else
        {
            return this;
        }
    }

    public override string asString()
    {
        return "dive";
    }
}
