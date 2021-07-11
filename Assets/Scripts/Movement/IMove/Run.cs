using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Represents movement on the ground while jumping is not occurring.
/// </summary>
public class Run : AMove
{
    float horizVel;
    bool jumpPending;
    bool vertBoostPending;
    bool timeBetweenJumpsBreaksTJ;

    /// <summary>
    /// Constructs a Run, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public Run(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        if (horizVel < 0) horizVel = 0;
        this.horizVel = horizVel;
        mii.OnJump.AddListener(() => jumpPending = true);
        mii.OnVertBoostRelease.AddListener(() => vertBoostPending = true);
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitToBreakTimeBetweenJumps());
    }

    public override void AdvanceTime()
    {
        // Horizontal
        if (mii.PressingBoost())
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    movementSettings.GroundBoostMaxSpeedX,
                    movementSettings.GroundBoostSensitivityX,
                    movementSettings.GroundBoostGravityX);
        }
        else if (horizVel > movementSettings.MaxSpeed)
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.RunSensitivityX,
                    movementSettings.RunGravityX);
        }
        else
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.RunSensitivityX,
                    movementSettings.RunGravityX);
        }
    }

    // Call with MonobehaviourUtils for a coroutine
    IEnumerator WaitToBreakTimeBetweenJumps()
    {
        yield return new WaitForSeconds(movementSettings.TjMaxTimeBtwnJumps);
        timeBetweenJumpsBreaksTJ = true;
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return ForwardMovement(horizVel);
    }

    public override float GetVertSpeedThisFrame()
    {
        return -0.5f;
    }

    public override float GetRotationSpeed()
    {
        if (mii.PressingBoost())
        {
            return movementSettings.GroundBoostRotationSpeed;
        }
        return (horizVel < movementSettings.InstantRotationSpeed) ?
            float.MaxValue : movementSettings.GroundRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (vertBoostPending)
        {
            return new VertAirBoost(mii, mi, mii.VertBoostTimeCharged() / movementSettings.VertBoostMaxChargeTime, movementSettings, horizVel);
        }
        if (PlayerSlopeHandler.BeyondMaxAngle && mi.TouchingGround())
        {
            return new Slide(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        else if (horizVel == 0)
        {
            return new Idle(mii, mi, movementSettings);
        }
        if ((jumpPending || mii.InReverseCoyoteTime()) && mi.NextJumpIsDoubleJump())
        {
            return new DoubleJump(mii, mi, movementSettings, horizVel);
        }
        if ((jumpPending || mii.InReverseCoyoteTime()) && mi.NextJumpIsTripleJump())
        {
            return new TripleJump(mii, mi, movementSettings, horizVel);
        }
        if (jumpPending || mii.InReverseCoyoteTime())
        {
            return new Jump(mii, mi, movementSettings, horizVel);
        }
        if (!mi.TouchingGround())
        {
            return new Fall(mii, mi, movementSettings, horizVel, true);
        }
        if (mii.HardTurnInput() && mii.GetHorizontalInput().magnitude > 0
            && horizVel > movementSettings.HardTurnMinSpeed
            && !mii.PressingBoost())
        {
            return new HardTurn(mii, mi, movementSettings, horizVel);
        }
        // todo make ground boost possible

        return this;
    }

    public override string AsString()
    {
        return "run";
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return mii.GetHorizDissonance() > movementSettings.TjMaxDissonance
            || mii.GetHorizontalInput().magnitude < movementSettings.TjMinHorizInputMagnitude
            || timeBetweenJumpsBreaksTJ;
    }

    public override bool AdjustToSlope()
    {
        return true;
    }
}
