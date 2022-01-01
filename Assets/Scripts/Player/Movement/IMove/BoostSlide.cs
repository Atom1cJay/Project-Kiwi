using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSlide : AMove
{
    float horizVel;
    bool fwdInput;
    readonly bool allowRefresh;
    FromStatus fromStatus;

    bool boostChargePending;
    //bool bonkPending;

    public BoostSlide(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel, bool allowRefresh) : base(ms, mi, mii)
    {
        this.allowRefresh = allowRefresh;
        this.horizVel = horizVel;
        mii.OnHorizBoostCharge.AddListener(() => boostChargePending = true);
        //mi.OnCharContTouchSomething.AddListener(() => bonkPending = true);
    }

    public BoostSlide(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel, bool allowRefresh, FromStatus fromStatus) : this(mii, mi, ms, horizVel, allowRefresh)
    {
        this.fromStatus = fromStatus;
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
        return -1f;
    }

    public override RotationInfo GetRotationInfo()
    {
        float rotSpeedProp = horizVel / movementSettings.BoostSlideMaxSpeedForMinRotation;
        float rotSpeed = Mathf.Lerp(movementSettings.BoostSlideMaxRotationSpeed, movementSettings.BoostSlideMinRotationSpeed, rotSpeedProp);
        return new RotationInfo(rotSpeed, false);
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
            return new Run(mii, mi, movementSettings, ForwardMovement(horizVel), FromStatus.FromSlide);
        }
        if (horizVel <= 0)
        {
            return new Idle(mii, mi, movementSettings);
        }
        if (boostChargePending && allowRefresh)
        {
            return new HorizGroundBoostCharge(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (mi.BonkDetectorTouching())
        {
            return new Knockback(mii, mi, movementSettings, Vector3.zero, ForwardMovement(horizVel));
        }
        /*
        if (bonkPending)
        {

        }
        */
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
        if (fromStatus == FromStatus.FromBoostCharge)
        {
            return new MovementParticleInfo.MovementParticles[] { MovementParticleInfo.Instance.SlidingBoost, MovementParticleInfo.Instance.SlidingTracks };
        }
        else
        {
            return new MovementParticleInfo.MovementParticles[] { MovementParticleInfo.Instance.Sliding, MovementParticleInfo.Instance.SlidingTracks };
        }
    }

    public override bool Pausable()
    {
        return true;
    }
}
