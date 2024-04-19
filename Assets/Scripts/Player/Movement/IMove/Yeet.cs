using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yeet : AMove
{
    float yVel;
    bool glidePending;

    public Yeet(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(ms, mi, mii)
    {
        yVel = movementSettings.YeetInitYVel;
        mii.OnGlide.AddListener(() => glidePending = true);
    }

    public override void AdvanceTime()
    {
        yVel -= movementSettings.YeetYGravity * Time.deltaTime;
    }

    public override bool AdjustToSlope()
    {
        return false;
    }

    public override string AsString()
    {
        return "yeet";
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return -ForwardMovement(movementSettings.YeetInitXVel);
    }

    public override IMove GetNextMove()
    {
        if (mi.TouchingGround())
        {
            return new Idle(mii, mi, movementSettings);
        }
        if (glidePending)
        {
            return new Glidev3(mii, mi, movementSettings, -ForwardMovement(movementSettings.YeetInitXVel));
        }
        return this;
    }

    public override RotationInfo GetRotationInfo()
    {
        return new RotationInfo(0, false);
    }

    public override float GetVertSpeedThisFrame()
    {
        return yVel;
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
