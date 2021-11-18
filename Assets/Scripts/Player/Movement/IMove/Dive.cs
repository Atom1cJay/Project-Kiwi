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
    bool swimPending;
    private bool receivedBasicHit;
    private Vector3 basicHitNormal;

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
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
        mi.ph.onBasicHit.AddListener((Vector3 basicHitNormal) => { receivedBasicHit = true; this.basicHitNormal = basicHitNormal; });
    }

    public override void AdvanceTime()
    {
        // Vertical
        vertVel -= movementSettings.DiveGravity * Time.deltaTime;
        // Horizontal
        if (mii.AirReverseInput())
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
        if (mii.AirReverseInput())
        {
            return 0;
        }
        return movementSettings.DiveRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (receivedBasicHit)
        {
            return new Knockback(mii, mi, movementSettings, basicHitNormal, ForwardMovement(horizVel));
        }
        if (PlayerSlopeHandler.ShouldSlide && mi.TouchingGround())
        {
            return new Slide(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        else if (mi.TouchingGround() && mii.GetHorizontalInput().magnitude > 0)
        {
            return new Run(mii, mi, movementSettings, ForwardMovement(horizVel), FromStatus.FromAir);
        }
        else if (mi.TouchingGround() && mii.GetHorizontalInput().magnitude == 0)
        {
            return new Idle(mii, mi, movementSettings, FromStatus.FromAir);
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

    public override Attack GetAttack()
    {
        return movementSettings.DiveAttack;
    }
}
