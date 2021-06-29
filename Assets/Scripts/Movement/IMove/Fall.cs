using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : AMove
{
    float horizVel;
    float vertVel;
    bool divePending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;
    bool hasInitiatedAirReverse; // Permanent once it starts TODO change?

    /// <summary>
    /// Constructs a Fall, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public Fall(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;
        vertVel = 0;
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnVertBoostCharge.AddListener(() => vertBoostChargePending = true);
        mii.OnHorizBoostCharge.AddListener(() => horizBoostChargePending = true);
    }

    public override void AdvanceTime()
    {
        // Horizontal
        if (mii.AirReverseInput())
        {
            hasInitiatedAirReverse = true;
        }

        if (hasInitiatedAirReverse)
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    -mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirReverseSensitivityX,
                    movementSettings.AirReverseGravityX);
            if (horizVel < 0) horizVel = 0;
        }
        else if (horizVel > movementSettings.MaxSpeed)
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirSensitivityX,
                    movementSettings.AirGravityXOverTopSpeed);
        }
        else
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirSensitivityX,
                    movementSettings.AirGravityX);
        }
        // Vertical
        vertVel -= movementSettings.DefaultGravity * Time.deltaTime;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return horizVel;
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override float GetRotationSpeed()
    {
        return hasInitiatedAirReverse ? 0 : movementSettings.AirRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (mi.TouchingGround())
        {
            return new Run(mii, mi, movementSettings, horizVel);
        }
        if (divePending)
        {
            return new Dive(mii, mi, movementSettings);
        }
        if (horizBoostChargePending)
        {
            return new HorizAirBoostCharge(mii, mi, movementSettings, vertVel, horizVel);
        }
        if (vertBoostChargePending)
        {
            return new VertAirBoostCharge(mii, mi, movementSettings, vertVel, horizVel);
        }

        return this;
    }

    public override string AsString()
    {
        return "fall";
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
        return true;
    }
}
