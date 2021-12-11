using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSlideFall : AMove
{
    float vertVel;
    float horizVel;
    readonly bool allowRefresh;
    bool objectHitPending;

    public BoostSlideFall(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel, bool allowRefresh) : base(ms, mi, mii)
    {
        this.allowRefresh = allowRefresh;
        vertVel = 0;
        this.horizVel = horizVel;
        mi.OnCharContTouchSomething.AddListener(() => objectHitPending = true);
    }

    public override void AdvanceTime()
    {
        vertVel -= movementSettings.DefaultGravity * Time.deltaTime;
        horizVel -= movementSettings.BoostSlideSpeedDecRate * Time.deltaTime;
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
        return vertVel;
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
        if (mi.TouchingGround())
        {
            return new BoostSlide(mii, mi, movementSettings, horizVel, allowRefresh);
        }
        if (objectHitPending)
        {
            Vector3 kbVector = new Vector3(-ForwardMovement(horizVel).x, 0, -ForwardMovement(horizVel).y);
            return new Knockback(mii, mi, movementSettings, kbVector, ForwardMovement(horizVel));
        }
        return this;
    }

    public override string AsString()
    {
        return "boostslidefall";
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
}
