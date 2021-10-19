using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSlideFall : AMove
{
    float vertVel;
    float horizVel;
    bool swimPending;
    readonly bool allowRefresh;

    public BoostSlideFall(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel, bool allowRefresh) : base(ms, mi, mii)
    {
        this.allowRefresh = allowRefresh;
        vertVel = 0;
        this.horizVel = horizVel;
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
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
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (PlayerSlopeHandler.ShouldSlide)
        {
            return new Slide(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (mi.TouchingGround())
        {
            return new BoostSlide(mii, mi, movementSettings, horizVel, allowRefresh);
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
