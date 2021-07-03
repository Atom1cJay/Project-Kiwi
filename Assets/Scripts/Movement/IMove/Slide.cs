using System;
using UnityEngine;

public class Slide : AMove
{
    Vector2 horizVector;

    public Slide(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(ms, mi, mii)
    {
        // Default Constructor
    }

    public override bool AdjustToSlope()
    {
        return true;
    }

    public override void AdvanceTime()
    {
        float xDeriv = PlayerSlopeHandler.XDeriv;
        float zDeriv = PlayerSlopeHandler.ZDeriv;
        float xInc = Mathf.Cos(Mathf.Asin(xDeriv)) * -Mathf.Sign(xDeriv);
        float zInc = Mathf.Cos(Mathf.Asin(zDeriv)) * -Mathf.Sign(zDeriv);
        horizVector += new Vector2(xInc, zInc) * movementSettings.SlideSpeed;
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
            return new Fall(mii, mi, movementSettings, horizVector.magnitude, false);
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
        return -10;
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
