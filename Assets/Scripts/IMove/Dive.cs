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
    readonly MovementInputInfo mii;
    readonly MovementInfo mi;

    public Dive(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = movementSettings.DiveInitVel;
        this.mii = mii;
        this.mi = mi;
    }

    public override void AdvanceTime()
    {
        // The only changing information within this move is vertVel
        vertVel -= movementSettings.DiveGravity * Time.deltaTime;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return movementSettings.DiveSpeedX;
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override float GetRotationSpeed()
    {
        return movementSettings.DiveRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (mi.TouchingGround())
        {
            return new Run(mm, mii, mi, movementSettings);
        }
        return this;
    }

    public override string AsString()
    {
        return "dive";
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return true;
    }
}
