using System;
using UnityEngine;
using System.Collections;

public class HorizAirBoost : AMove
{
    float vertVel;
    float timeLeft;
    MovementInputInfo mii;
    MovementInfo mi;
    bool divePending;

    public HorizAirBoost(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, float timeLeft, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = 0;
        this.timeLeft = timeLeft;
        this.mii = mii;
        mii.OnDiveInput.AddListener(() => divePending = true);
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

    public override float GetRotationThisFrame()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            return new Fall(mm, mii, mi, movementSettings);
        }
        if (mi.touchingGround())
        {
            return new Run(mm, mii, mi, movementSettings);
        }
        if (divePending)
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
