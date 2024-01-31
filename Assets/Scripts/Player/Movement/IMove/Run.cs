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
    bool pushPending;
    readonly FromStatus fromStatus = FromStatus.Null;
    bool spawnedParticlesFirstFrame;

    bool inBoopingWaterState; // Making water boop particles?

    /// <summary>
    /// Constructs a Run, initializing the objects that hold all the
    /// information it needs to function. FromIdle will be false by
    /// default.
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
        mii.OnPushPress.AddListener(() => pushPending = true);
    }

    // Overload constructor for explicitly giving information about where this
    // move is coming from
    public Run(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector, FromStatus fromStatus) : this(mii, mi, ms, horizVector)
    {
        this.fromStatus = fromStatus;
        if (this.fromStatus == FromStatus.FromAir) // Potential land event
        {

            Landable potentialLandScript = mi.GetGroundDetector().CollidingWith()?.GetComponent<Landable>();
            if (potentialLandScript != null)
            {
                potentialLandScript.BroadcastLandEvent();
            }
        }
    }

    public override void AdvanceTime()
    {
        // Horizontal 
        if (horizVel > movementSettings.MaxSpeed)
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
            return -10f;
        }
        return -1f; // To prevent floating point imprecision taking you off ground
    }

    public override RotationInfo GetRotationInfo()
    {
        if (horizVel < movementSettings.InstantRotationSpeed)
        {
            return new RotationInfo(float.MaxValue, false);
        }
        if (horizVel <= movementSettings.MaxSpeed)
        {
            return new RotationInfo(movementSettings.GroundRotationSpeed, true);
        }
        float propToAbsoluteMax = (horizVel - movementSettings.MaxSpeed) / (movementSettings.MaxSpeedAbsolute - movementSettings.MaxSpeed);
        return new RotationInfo(Mathf.Lerp(movementSettings.GroundRotationSpeed, movementSettings.GroundRotationSpeedMaxXSpeed, propToAbsoluteMax), true);
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
        if (pushPending)
        {
            return new HorizGroundBoostCharge(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (PlayerSlopeHandler.ShouldSlide)
        {
            return new Slide(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        else if (horizVel == 0 && mii.GetHorizontalInput().magnitude == 0)
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

    public override MovementParticleInfo.MovementParticles[] GetParticlesToSpawn()
    {
        if (!inBoopingWaterState && mi.BoopingWater())
        {
            inBoopingWaterState = true;
            return new MovementParticleInfo.MovementParticles[]
                { MovementParticleInfo.Instance.WaterWalking };
        }

        if (spawnedParticlesFirstFrame) // Everything from here on out should only spawn the first frame
        {
            return null;
        }

        spawnedParticlesFirstFrame = true;

        switch (fromStatus)
        {
            case FromStatus.FromSlide:
                return new MovementParticleInfo.MovementParticles[]
                { MovementParticleInfo.Instance.Walking, MovementParticleInfo.Instance.WalkingSmoke };
            case FromStatus.FromSwim:
            case FromStatus.FromHardTurn:
            case FromStatus.FromIdle:
                return new MovementParticleInfo.MovementParticles[]
                { MovementParticleInfo.Instance.Accel, MovementParticleInfo.Instance.Walking, MovementParticleInfo.Instance.WalkingSmoke };
            case FromStatus.FromAir:
            case FromStatus.FromGlide:
                return new MovementParticleInfo.MovementParticles[]
                { MovementParticleInfo.Instance.Landing, MovementParticleInfo.Instance.LandingImpact, MovementParticleInfo.Instance.Walking, MovementParticleInfo.Instance.WalkingSmoke };
            default:
                return null;
        }
    }

    public override MovementParticleInfo.MovementParticles[] GetParticlesToStop()
    {
        if (inBoopingWaterState && !mi.BoopingWater())
        {
            inBoopingWaterState = false;
            return new MovementParticleInfo.MovementParticles[]
                { MovementParticleInfo.Instance.WaterWalking };
        }
        return null;
    }

    public override bool Pausable()
    {
        return true;
    }
}
