using System;
using UnityEngine;
using System.Collections;

public class HorizAirBoost : AMove
{
    float vertVel;
    float timeLeft;
    MovementInputInfo mii;
    MovementInfo mi;

    public HorizAirBoost(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, float timeLeft, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = 0;
        this.timeLeft = timeLeft;
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return movementSettings.HorizBoostSpeedX;
    }

    public override float GetVertSpeedThisFrame()
    {
        vertVel -= movementSettings.HorizBoostGravity * Time.deltaTime;
        return vertVel;
    }

    public override IMove GetNextMove()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            return new Fall(mm, mii, mi, movementSettings);
        }
        if (mm.IsOnGround())
        {
            return new Run(mm, mii, mi, movementSettings);
        }
        if (mm.IsAirDiving())
        {
            return new Dive(mm, mii, mi, movementSettings);
        }

        return this;
    }

    public override string asString()
    {
        return "horizairboost";
    }
}
