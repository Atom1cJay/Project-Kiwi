using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizGroundBoostCharge : AMove
{
    float horizVel;
    float timeCharging;
    readonly float maxTimeToCharge;
    bool boostReleasePending;

    /// <summary>
    /// Constructs a HorizAirBoostCharge, initializing the objects that hold all
    /// the information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="vertVel">The vertical speed moving into this move</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public HorizGroundBoostCharge(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(ms, mi, mii)
    {
        horizVel = GetSharedMagnitudeWithPlayerAngle(horizVector);
        timeCharging = 0;
        maxTimeToCharge = movementSettings.HorizBoostMaxChargeTime;
        mii.OnHorizBoostRelease.AddListener(() => boostReleasePending = true);
    }

    public override void AdvanceTime()
    {
        timeCharging += Time.deltaTime;
        // Horizontal
        horizVel = InputUtils.SmoothedInput(
            horizVel, 0, 0, movementSettings.HorizBoostChargeGravityXGround);
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
        if (!mi.TouchingGround() && !PlayerSlopeHandler.GroundInProximity)
        {
            return new HorizAirBoostCharge(mii, mi, movementSettings, 0, ForwardMovement(horizVel));
        }
        if (timeCharging > maxTimeToCharge || boostReleasePending)
        {
            float propCharged = timeCharging / maxTimeToCharge;
            float wantedVel = movementSettings.HorizBoostMinSpeedGroundX + (propCharged * (movementSettings.HorizBoostMaxSpeedGroundX - movementSettings.HorizBoostMinSpeedGroundX));
            return new BoostSlide(mii, mi, movementSettings, wantedVel, true, FromStatus.FromBoostCharge);
        }
        else
        {
            return this;
        }
    }

    public override string AsString()
    {
        return "horizgroundboostcharge";
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

    public override MovementParticleInfo.MovementParticles[] GetParticlesToSpawn()
    {
        return new MovementParticleInfo.MovementParticles[] { MovementParticleInfo.Instance.Sliding, MovementParticleInfo.Instance.SlidingTracks };
    }

    public override bool Pausable()
    {
        return true;
    }
}
