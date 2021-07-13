using System;
using UnityEngine;

public class Slide : AMove
{
    Vector2 horizVector;

    public Slide(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(ms, mi, mii)
    {
        this.horizVector = horizVector;
    }

    public override bool AdjustToSlope()
    {
        return true;
    }

    public override void AdvanceTime()
    {
        float xDeriv = PlayerSlopeHandler.XDeriv;
        float zDeriv = PlayerSlopeHandler.ZDeriv;
        Vector2 toChange = new Vector2(xDeriv, zDeriv);
        horizVector -= toChange * movementSettings.SlideForce;
        if (horizVector.magnitude > movementSettings.SlideMaxSpeed)
        {
            horizVector = horizVector.normalized * movementSettings.SlideMaxSpeed;
        }
    }

    public override string AsString()
    {
        return "slide";
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return horizVector;
    }

    public override IMove GetNextMove()
    {
        if (!mi.TouchingGround())
        {
            return new HelplessFall(mii, mi, movementSettings, horizVector);
        }
        if (mi.TouchingGround() && !PlayerSlopeHandler.BeyondMaxAngle)
        {
            return new SlideRecovery(mii, mi, movementSettings, horizVector);
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
