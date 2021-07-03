using System;
using UnityEngine;
using System.Collections;

public class HorizAirBoost : AMove
{
    readonly float gravity;
    float vertVel;
    float horizVel;
    bool divePending;
    bool groundPoundPending;
    bool airReverseActivated;

    /// <summary>
    /// Constructs a HorizAirBoost, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    public HorizAirBoost(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float propCharged) : base(ms, mi, mii)
    {
        gravity = movementSettings.HorizBoostMinGravity;
        horizVel = movementSettings.HorizBoostMinActivationX + (propCharged * (movementSettings.HorizBoostMaxActivationX - movementSettings.HorizBoostMinActivationX));
        vertVel = 0;
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
    }

    public override void AdvanceTime()
    {
        // Vertical
        vertVel -= gravity * Time.deltaTime;
        // Horizontal
        if (mii.AirReverseInput())
        {
            airReverseActivated = true;
        }
        if (!airReverseActivated)
        {
            horizVel = InputUtils.SmoothedInput(
                horizVel,
                movementSettings.MaxSpeed * mii.GetHorizontalInput().magnitude,
                0, movementSettings.HorizBoostNonAirReverseGravity);
        }
        else
        {
            horizVel = InputUtils.SmoothedInput(
                horizVel, 0, 0, movementSettings.HorizBoostAirReverseGravity);   
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
        return airReverseActivated ? 0 : movementSettings.HorizBoostRotation;
    }

    public override IMove GetNextMove()
    {
        if (mi.TouchingGround())
        {
            return new Run(mii, mi, movementSettings, horizVel);
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
        return "horizairboost";
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
