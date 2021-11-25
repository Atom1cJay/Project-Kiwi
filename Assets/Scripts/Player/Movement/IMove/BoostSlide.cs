using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSlide : AMove
{
    float horizVel;
    bool boostChargePending;
    bool fwdInput;
    readonly bool allowRefresh;

    public BoostSlide(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel, bool allowRefresh) : base(ms, mi, mii)
    {
        this.allowRefresh = allowRefresh;
        this.horizVel = horizVel;
        mii.OnHorizBoostCharge.AddListener(() => boostChargePending = true);
    }

    public override void AdvanceTime()
    {
        Vector2 horizInput = mii.GetHorizontalInput();
        fwdInput = horizInput.magnitude > 0 && mii.GetHorizDissonance() < movementSettings.BoostSlideMaxDissonanceForHoldingFwd;
        // TODO proper charge system
        if (fwdInput)
        {
            horizVel -= movementSettings.BoostSlideSpeedDecRate * Time.deltaTime;
        }
        else
        {
            horizVel -= movementSettings.BoostSlideSpeedDecRateNoInput * Time.deltaTime;
        }
        if (horizVel < 0)
        {
            horizVel = 0;
        }
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
        return -0.5f;
    }

    public override float GetRotationSpeed()
    {
        float rotSpeedProp = horizVel / movementSettings.BoostSlideMaxSpeedForMinRotation;
        return Mathf.Lerp(movementSettings.BoostSlideMaxRotationSpeed, movementSettings.BoostSlideMinRotationSpeed, rotSpeedProp);
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
        if (PlayerSlopeHandler.ShouldSlide)
        {
            return new Slide(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (mii.GetInputActions().Player.Jump.ReadValue<float>() > 0)
        {
            return new BoostSlideHop(mii, mi, movementSettings, horizVel);
        }
        if (!mi.TouchingGround() && !PlayerSlopeHandler.GroundInProximity)
        {
            return new BoostSlideFall(mii, mi, movementSettings, horizVel, allowRefresh);
        }
        if (horizVel <= movementSettings.BoostSlideEndSpeedHoldingFwd && fwdInput)
        {
            return new Run(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (horizVel <= 0)
        {
            return new Idle(mii, mi, movementSettings);
        }
        if (boostChargePending && allowRefresh)
        {
            return new HorizGroundBoostCharge(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        return this;
    }

    public override string AsString()
    {
        return "boostslide";
    }

    public override bool AdjustToSlope()
    {
        return true;
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return true;
    }

    public override MovementParticleInfo.MovementParticles[] GetParticlesToSpawn()
    {
        return new MovementParticleInfo.MovementParticles[] { MovementParticleInfo.Instance.Sliding, MovementParticleInfo.Instance.SlidingTracks };
    }
}
