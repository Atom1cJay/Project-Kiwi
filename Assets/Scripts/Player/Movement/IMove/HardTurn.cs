﻿using System;
using UnityEngine;

public class HardTurn : AMove
{
    float horizVel;
    float timeLeft;
    bool jumpInputPending;

    /// <summary>
    /// Constructs a HardTurn, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public HardTurn(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;
        timeLeft = movementSettings.HardTurnTime;
        mii.OnJump.AddListener(() => jumpInputPending = true);
    }

    public override void AdvanceTime()
    {
        // Meta
        timeLeft -= Time.deltaTime;
        // Horizontal
        horizVel = InputUtils.SmoothedInput(
            horizVel, 0, 0, movementSettings.HardTurnGravityX);
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return ForwardMovement(horizVel);
    }

    public override float GetVertSpeedThisFrame()
    {
        return -3;
    }

    public override RotationInfo GetRotationInfo()
    {
        return new RotationInfo(0, false);
    }

    public override IMove GetNextMove()
    {
        // Handle Feedback Moves
        IMove feedbackMove = GetFeedbackMove(ForwardMovement(horizVel));
        if (feedbackMove != null)
        {
            return feedbackMove;
        }
        // Handle Everything Else
        if (timeLeft < 0)
        {
            return new Run(mii, mi, movementSettings, ForwardMovement(horizVel), FromStatus.FromHardTurn);
        }
        if (jumpInputPending || mii.InReverseCoyoteTime())
        {
            return new Jump(mii, mi, movementSettings, horizVel);
        }
        if (!mi.TouchingGround())
        {
            return new Fall(mii, mi, movementSettings, ForwardMovement(horizVel), false);
        }
        return this;
    }

    public override string AsString()
    {
        return "hardturn";
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
        return true;
    }

    public override bool Pausable()
    {
        return true;
    }
}
