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
    bool swimPending;
    bool pushPending;

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
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitToBreakTimeBetweenJumps());
        mii.OnPushPress.AddListener(() => pushPending = true);
    }

    /*
    // Push input received, now start push
    private void BeginPushMode()
    {
        base.StartPushMaintainTime();
        // Calculate speed as it would be if increased at standard rate
        float horizVelIncreased = horizVel + movementSettings.PushSpeedIncMax;
        horizVelIncreased = Mathf.Clamp(horizVelIncreased, 0, movementSettings.MaxSpeedPushed);
        // Should we do that speed or just the max speed (if std inc speed is too low)
        horizVel = Mathf.Max(movementSettings.MaxSpeed, horizVelIncreased);
    }
    */

    public override void AdvanceTime()
    {
        // Should the push maintain end early?
        if (mii.GetHorizontalInput().magnitude < 1)
        {
            base.EndPushMaintainTime();
        }
        // Horizontal 
        if (pushMaintainTimeLeft > 0) // If maintaining push speed
        {
            // Just wait, don't change horizVel
        }
        else if (horizVel > movementSettings.MaxSpeed)
        {
            // TODO change?
            float gravityToUse = (mii.GetHorizontalInput().magnitude == 0) ?
                movementSettings.RunGravityXOverTopSpeedNoInput : movementSettings.RunGravityXOverTopSpeed;
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.RunSensitivityX,
                    gravityToUse);
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
        if (horizVel > movementSettings.MaxSpeedAbsolute)
        {
            horizVel = movementSettings.MaxSpeedAbsolute;
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
        return -0.5f; // To prevent floating point imprecision taking you off ground
    }

    public override float GetRotationSpeed()
    {
        if (horizVel < movementSettings.InstantRotationSpeed)
        {
            return float.MaxValue;
        }
        if (horizVel <= movementSettings.MaxSpeed)
        {
            return movementSettings.GroundRotationSpeed;
        }
        // How far is speed between max speed and abs max speed?
        float propToAbsoluteMax = (horizVel - movementSettings.MaxSpeed) / (movementSettings.MaxSpeedAbsolute - movementSettings.MaxSpeed);
        return Mathf.Lerp(movementSettings.GroundRotationSpeed, movementSettings.GroundRotationSpeedMaxXSpeed, propToAbsoluteMax);
        /*
        if (mii.PressingBoost())
        {
            return movementSettings.GroundBoostRotationSpeed;
        }
        */
        /*
        return (horizVel < movementSettings.InstantRotationSpeed) ?
            float.MaxValue : movementSettings.GroundRotationSpeed;
        */
    }

    public override IMove GetNextMove()
    {
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (pushPending)
        {
            return new HorizGroundBoostCharge(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (PlayerSlopeHandler.ShouldSlide)
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
            || Mathf.Min(horizVel, mi.GetEffectiveSpeed().magnitude) < movementSettings.TjMaxBreakSpeed;
    }

    public override bool AdjustToSlope()
    {
        return true;
    }
}
