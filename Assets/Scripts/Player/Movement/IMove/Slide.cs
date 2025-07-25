﻿using System;
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
        Vector2 toChange = new Vector2(Mathf.Sin(Mathf.Atan2(xDeriv, 1)), Mathf.Sin(Mathf.Atan2(zDeriv, 1)));
        float yMovement = (xDeriv * horizVector.x) + (zDeriv * horizVector.y);
        float slideForce = (yMovement > 0) ? movementSettings.SlideForceToZero : movementSettings.SlideForceNegative;
        horizVector -= toChange * slideForce;
        Vector3 totalMovement = new Vector3(horizVector.x, yMovement, horizVector.y);
        if (totalMovement.magnitude > movementSettings.SlideMaxSpeed)
        {
            horizVector.x = totalMovement.normalized.x * movementSettings.SlideMaxSpeed;
            horizVector.y = totalMovement.normalized.z * movementSettings.SlideMaxSpeed;
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
        // Handle Feedback Moves
        IMove feedbackMove = GetFeedbackMove(horizVector);
        if (feedbackMove != null)
        {
            return feedbackMove;
        }
        // Handle Everything Else
        if (mi.TouchingGround() && !PlayerSlopeHandler.ShouldSlide)
        {
            return new SlideRecovery(mii, mi, movementSettings, horizVector);
        }
        return this;
    }

    public override RotationInfo GetRotationInfo()
    {
        return new RotationInfo(0, false);
    }

    public override float GetVertSpeedThisFrame()
    {
        return -15;
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
