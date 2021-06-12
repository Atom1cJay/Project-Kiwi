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
    MovementSettings ms;
    MovementInputInfo mii;
    MovementInfo mi;

    public Dive(MovementMaster mm, MovementSettings ms, MovementInputInfo mii, MovementInfo mi) : base(mm)
    {
        vertVel = ms.diveInitVel;
        this.ms = ms;
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return ms.diveSpeedX;
    }

    public override float GetVertSpeedThisFrame()
    {
        vertVel -= ms.diveGravity * Time.deltaTime;
        return vertVel;
    }

    public override IMove GetNextMove()
    {
        if (mm.IsOnGround())
        {
            return new Run(mm, ms, mii, mi);
        }
        else
        {
            return this;
        }
    }
}
