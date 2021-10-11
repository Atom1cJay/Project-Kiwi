using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSlideFall : AMove
{
    float vertVel;
    float horizVel;

    public BoostSlideFall(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        vertVel = 0;
        this.horizVel = horizVel;
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
        return movementSettings.BoostSlideRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (mi.TouchingGround())
        {
            return new BoostSlide(mii, mi, movementSettings, horizVel);
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
