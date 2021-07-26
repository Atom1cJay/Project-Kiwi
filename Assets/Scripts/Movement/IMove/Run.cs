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
    bool timeBetweenJumpsBreaksTJ;

    /// <summary>
    /// Constructs a Run, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVector">The horizontal velocity going into this move</param>
    public Run(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(ms, mi, mii)
    {
        horizVel = GetSharedMagnitudeWithPlayerAngle(horizVector);
        mii.OnJump.AddListener(() => jumpPending = true);
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitToBreakTimeBetweenJumps());
    }

    public override void AdvanceTime()
    {
        // Horizontal
        if (mii.PressingBoost() && horizVel >= movementSettings.MaxSpeed)
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
        if (!mi.TouchingGround() && PlayerSlopeHandler.GroundInProximity)
        {
            return -10;
        }
        // To account for any very slight floating point errors in the
        // angle, and to make it so the player doesn't go straight horizontally
        // when running onto another angle
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
            return new DoubleJump(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if ((jumpPending || mii.InReverseCoyoteTime()) && mi.NextJumpIsTripleJump())
        {
            return new TripleJump(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (jumpPending || mii.InReverseCoyoteTime())
        {
            return new Jump(mii, mi, movementSettings, horizVel);
        }
        if (!mi.TouchingGround() && !PlayerSlopeHandler.GroundInProximity)
        {
            return new Fall(mii, mi, movementSettings, ForwardMovement(horizVel), true);
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
            || timeBetweenJumpsBreaksTJ
            || Mathf.Min(horizVel, mi.GetEffectiveSpeed()) < movementSettings.TjMaxBreakSpeed;
    }

    public override bool AdjustToSlope()
    {
        return true;
    }
}
