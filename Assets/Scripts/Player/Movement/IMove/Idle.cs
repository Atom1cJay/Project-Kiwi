using System;
using UnityEngine;

public class Idle : AMove
{
    bool jumpPending;
    FromStatus fromStatus;

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

    // Override constructor to include fromStatus
    public Idle(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, FromStatus fromStatus) : this(mii, mi, ms)
    {
        this.fromStatus = fromStatus;
    }

    public override void AdvanceTime()
    {
        // Nothing changes over time
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return Vector2.zero;
    }

    public override float GetRotationSpeed()
    {
        return float.MaxValue; // Instant
    }

    public override float GetVertSpeedThisFrame()
    {
        if (!mi.TouchingGround() && PlayerSlopeHandler.GroundInProximity)
        {
            return -10;
        }
        return 0;
    }

    public override IMove GetNextMove()
    {
        // Handle Feedback Moves
        IMove feedbackMove = GetFeedbackMove(Vector2.zero);
        if (feedbackMove != null)
        {
            return feedbackMove;
        }
        // Handle Everything Else
        if (PlayerSlopeHandler.ShouldSlide && mi.TouchingGround())
        {
            return new Slide(mii, mi, movementSettings, Vector2.zero);
        }
        if (mii.GetHorizontalInput().magnitude != 0)
        {
            return new Run(mii, mi, movementSettings, Vector2.zero, FromStatus.FromIdle);
        }
        if (jumpPending || mii.InReverseCoyoteTime())
        {
            return new Jump(mii, mi, movementSettings, 0);
        }
        if (!mi.TouchingGround() && !PlayerSlopeHandler.GroundInProximity)
        {
            return new Fall(mii, mi, movementSettings, Vector2.zero, true);
        }

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

    public override bool AdjustToSlope()
    {
        return true;
    }

    public override MovementParticleInfo.MovementParticles GetParticlesToSpawn()
    {
        if (fromStatus == FromStatus.FromAir)
        {
            return MovementParticleInfo.Instance.Landing;
        }
        return null;
    }
}
