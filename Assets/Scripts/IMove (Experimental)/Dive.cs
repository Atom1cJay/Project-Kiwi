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

    public Dive(HorizontalMovement hm, VerticalMovement vm, MovementMaster mm) : base(hm, vm, mm)
    {
        vertVel = vm.diveInitVel;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return hm.diveSpeed;
    }

    public override float GetVertSpeedThisFrame()
    {
        vertVel -= vm.diveGravity * Time.fixedDeltaTime;
        return vertVel;
    }

    public override IMove GetNextMove()
    {
        if (mm.IsOnGround())
        {
            return new Run(hm, vm, mm);
        }
        else
        {
            return this;
        }
    }
}
