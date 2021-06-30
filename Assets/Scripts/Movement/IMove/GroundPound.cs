using System;
using System.Collections;
using UnityEngine;

public class GroundPound : AMove
{
    float timePassed;
    bool divePending;

    public GroundPound(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(ms, mi, mii)
    {
        mii.OnDiveInput.AddListener(() => divePending = true);
    }

    public override void AdvanceTime()
    {
        timePassed += Time.deltaTime;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return 0;
    }

    public override float GetVertSpeedThisFrame()
    {
        if (timePassed < movementSettings.GpSuspensionTime)
        {
            return 0;
        }
        return -movementSettings.GpDownSpeed;
    }

    public override float GetRotationSpeed()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        if (divePending)
        {
            return new Dive(mii, mi, movementSettings);
        }
        if (mi.TouchingGround())
        {
            return new Idle(mii, mi, movementSettings);
        }
        return this;
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
        return "groundpound";
    }
}
