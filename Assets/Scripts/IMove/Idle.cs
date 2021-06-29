﻿using System;

public class Idle : AMove
{
    bool jumpPending;

    /// <summary>
    /// Constructs a Idle, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    public Idle(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(ms, mi, mii)
    {
        mii.OnJump.AddListener(() => jumpPending = true);
    }

    public override void AdvanceTime()
    {
        // Nothing changes over time
    }

    public override float GetHorizSpeedThisFrame()
    {
        return 0;
    }

    public override float GetRotationSpeed()
    {
        return movementSettings.GroundRotationSpeed;
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        if (mii.GetHorizontalInput().magnitude != 0)
        {
            return new Run(mii, mi, movementSettings, 0);
        }
        if (jumpPending)
        {
            return new Jump(mii, mi, movementSettings, 0);
        }
        if (!mi.TouchingGround())
        {
            return new Fall(mii, mi, movementSettings, GetHorizSpeedThisFrame());
        }
        // todo make ground boost possible

        return this;
    }

    public override string AsString()
    {
        return "idle";
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
