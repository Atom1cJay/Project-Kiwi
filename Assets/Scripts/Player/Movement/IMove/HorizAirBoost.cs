using System;
using UnityEngine;
using System.Collections;

public class HorizAirBoost : AMove
{
    float gravity;
    float vertVel;
    float horizVel;
    bool divePending;
    bool groundPoundPending;
    bool swimPending;

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
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
    }

    public override void AdvanceTime()
    {
        // Vertical
        gravity += movementSettings.HorizBoostGravityIncRate * Time.deltaTime;
        if (gravity > movementSettings.HorizBoostMaxGravity)
            gravity = movementSettings.HorizBoostMaxGravity;
        vertVel -= gravity * Time.deltaTime;
        // Horizontal
        if (mii.PressingBoost())
        {
            horizVel = InputUtils.SmoothedInput(
                horizVel,
                movementSettings.GroundBoostMaxSpeedX,
                movementSettings.HorizBoostToGroundBoostSensitivity, 0);
        }
        else if (!mii.AirReverseInput())
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
        if (divePending)
        {
            return float.MaxValue;
        }
        return mii.AirReverseInput() ? 0 : movementSettings.HorizBoostRotation;
    }

    public override IMove GetNextMove()
    {
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (mi.TouchingGround())
        {
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

    public override Attack GetAttack()
    {
        return movementSettings.HorizBoostAttack;
    }
}
