using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertAirBoost : AMove
{
    float vertVel;
    float horizVel;
    bool divePending;
    bool groundPoundPending;
    bool swimPending;

    /// <summary>
    /// Constructs a VertAirBoost, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public VertAirBoost(MovementInputInfo mii, MovementInfo mi, float propCharged, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;
        vertVel = movementSettings.VertBoostMinVel + (propCharged * (movementSettings.VertBoostMaxVel - movementSettings.VertBoostMinVel));
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
    }

    public override void AdvanceTime()
    {
        // Vertical
        vertVel -= movementSettings.VertBoostGravity * Time.deltaTime;
        // Horizontal
        horizVel = Math.Min(horizVel, mi.GetEffectiveSpeed().magnitude);
        if (mii.AirReverseInput())
        {
            horizVel = InputUtils.SmoothedInput(
                horizVel,
                -mii.GetHorizontalInput().magnitude * movementSettings.VertBoostMaxSpeedX,
                movementSettings.AirSensitivityX,
                movementSettings.AirGravityX);
        }
        else
        {
            horizVel = InputUtils.SmoothedInput(
                horizVel,
                mii.GetHorizontalInput().magnitude * movementSettings.VertBoostMaxSpeedX,
                movementSettings.AirSensitivityX,
                movementSettings.AirGravityX);
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
        if (divePending)
        {
            return float.MaxValue;
        }
        if (mii.AirReverseInput())
        {
            return 0;
        }
        return horizVel < movementSettings.InstantRotationSpeed ?
            float.MaxValue : movementSettings.VertBoostRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (mi.TouchingGround() && vertVel < 0)
        {
            if (horizVel < 0)
            {
                horizVel = 0;
            }
            return new Run(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        if (divePending)
        {
            return new Dive(mii, mi, movementSettings);
        }

        return this;
    }

    public override string AsString()
    {
        return "vertairboost";
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
