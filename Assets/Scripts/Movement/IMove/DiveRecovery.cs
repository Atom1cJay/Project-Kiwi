using System;
using UnityEngine;

public class DiveRecovery : AMove
{
    float horizVel;
    float timePassed;

    public DiveRecovery(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;
    }

    public override bool AdjustToSlope()
    {
        return true;
    }

    public override void AdvanceTime()
    {
        timePassed += Time.deltaTime;
    }

    public override string AsString()
    {
        return "diverecovery";
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return ForwardMovement(horizVel);
    }

    public override IMove GetNextMove()
    {
        if (!mi.TouchingGround())
        {
            return new Fall(mii, mi, movementSettings, ForwardMovement(horizVel), false);
        }
        if (timePassed > movementSettings.DiveRecoveryTime)
        {
            return new Run(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        return this; 
    }

    public override float GetRotationSpeed()
    {
        return 0;
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
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
