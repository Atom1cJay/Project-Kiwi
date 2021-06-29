using System;
using UnityEngine;
using System.Collections;

public class HorizAirBoost : AMove
{
    float vertVel;
    float timeLeft;
    bool divePending;

    /// <summary>
    /// Constructs a HorizAirBoost, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    public HorizAirBoost(MovementInputInfo mii, MovementInfo mi, float timeLeft, MovementSettingsSO ms) : base(ms, mi, mii)
    {
        vertVel = 0;
        this.timeLeft = timeLeft;
        mii.OnDiveInput.AddListener(() => divePending = true);
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
            return new Fall(mii, mi, movementSettings, GetHorizSpeedThisFrame());
        }
        if (mi.TouchingGround())
        {
            return new Run(mii, mi, movementSettings, GetHorizSpeedThisFrame());
        }
        if (divePending)
        {
            return new Dive(mii, mi, movementSettings);
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

    public override bool AdjustToSlope()
    {
        return false;
    }
}
