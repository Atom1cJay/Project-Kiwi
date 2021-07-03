using System;
using UnityEngine;

public class SlideRecovery : AMove
{
    Vector2 initHorizVector;
    Vector2 horizVector;

    public SlideRecovery(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(ms, mi, mii)
    {
        initHorizVector = horizVector;
        this.horizVector = horizVector;
    }

    public override bool AdjustToSlope()
    {
        return true;
    }

    public override void AdvanceTime()
    {
        horizVector -= (initHorizVector * movementSettings.SlideEndSpeed) * Time.deltaTime;
    }

    public override string AsString()
    {
        return "sliderecovery";
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return horizVector;
    }

    public override IMove GetNextMove()
    {
        if (PlayerSlopeHandler.BeyondMaxAngle)
        {
            return new Slide(mii, mi, movementSettings);
        }
        if (horizVector.magnitude < 0.1f)
        {
            return new Run(mii, mi, movementSettings, 0);
        }
        if (!mi.TouchingGround())
        {
            return new Fall(mii, mi, movementSettings, horizVector.magnitude, false);
        }
        return this;
    }

    public override float GetRotationSpeed()
    {
        return 0;
    }

    public override float GetVertSpeedThisFrame()
    {
        return -0.9f;
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
