using System;
using UnityEngine;
using System.Collections;

public class HorizAirBoost : AMove
{
    float vertVel;
    float timeLeft;
    readonly MovementInputInfo mii;
    readonly MovementInfo mi;
    bool divePending;

    public HorizAirBoost(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, float timeLeft, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = 0;
        this.timeLeft = timeLeft;
        this.mii = mii;
        mii.OnDiveInput.AddListener(() => divePending = true);
        this.mi = mi;
    }

    public override void AdvanceTime()
    {
        // Meta
        timeLeft -= Time.deltaTime;
        // Horizontal
        vertVel -= movementSettings.HorizBoostGravity * Time.deltaTime;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return movementSettings.HorizBoostSpeedX;
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override float GetRotationSpeed()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        if (timeLeft < 0)
        {
            return new Fall(mm, mii, mi, movementSettings);
        }
        if (mi.TouchingGround())
        {
            return new Run(mm, mii, mi, movementSettings);
        }
        if (divePending)
        {
            return new Dive(mm, mii, mi, movementSettings);
        }

        return this;
    }

    public override string AsString()
    {
        return "horizairboost";
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
