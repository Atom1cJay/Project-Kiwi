using System;
using UnityEngine;
using System.Collections;

public class HorizAirBoost : AMove
{
    float vertVel;
    float timeLeft;
    MovementSettings ms;
    MovementInputInfo mii;
    MovementInfo mi;

    public HorizAirBoost(MovementMaster mm, MovementSettings ms, MovementInputInfo mii, MovementInfo mi, float timeLeft) : base(mm)
    {
        vertVel = 0;
        this.timeLeft = timeLeft;
        this.ms = ms;
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return ms.horizBoostSpeedX;
    }

    public override float GetVertSpeedThisFrame()
    {
        vertVel -= ms.horizBoostGravity * Time.deltaTime;
        return vertVel;
    }

    public override IMove GetNextMove()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            return new Fall(mm, ms, mii, mi);
        }
        if (mm.IsOnGround())
        {
            return new Run(mm, ms, mii, mi);
        }
        if (mm.IsAirDiving())
        {
            return new Dive(mm, ms, mii, mi);
        }

        return this;
    }
}
