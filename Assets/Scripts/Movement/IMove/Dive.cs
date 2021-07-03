using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the dive move in the air
/// </summary>
public class Dive : AMove
{
    float horizVel;
    float vertVel;
    bool airReverseInitiated;

    /// <summary>
    /// Constructs a Dive, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    public Dive(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(ms, mi, mii)
    {
        vertVel = movementSettings.DiveInitVel;
        horizVel = movementSettings.DiveSpeedX;
    }

    public override void AdvanceTime()
    {
        // Vertical
        vertVel -= movementSettings.DiveGravity * Time.deltaTime;
        // Horizontal
        if (mii.AirReverseInput())
        {
            airReverseInitiated = true;
        }
        if (airReverseInitiated)
        {
            horizVel = InputUtils.SmoothedInput(horizVel, 0, 0, movementSettings.AirGravityX);
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
        if (airReverseInitiated)
        {
            return 0;
        }
        return movementSettings.DiveRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (PlayerSlopeHandler.BeyondMaxAngle && mi.TouchingGround())
        {
            return new Slide(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        else if (mi.TouchingGround())
        {
            return new DiveRecovery(mii, mi, movementSettings, horizVel);
        }
        return this;
    }

    public override string AsString()
    {
        return "dive";
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return true;
    }

    public override bool AdjustToSlope()
    {
        return false;
    }
}
