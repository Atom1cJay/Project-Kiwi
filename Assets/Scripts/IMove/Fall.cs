using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : AMove
{
    float horizVel;
    float vertVel;
    readonly MovementInputInfo mii;
    readonly MovementInfo mi;
    bool divePending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;
    bool hasInitiatedAirReverse; // Permanent once it starts TODO change?

    public Fall(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = 0;
        this.mii = mii;
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnVertBoostCharge.AddListener(() => vertBoostChargePending = true);
        mii.OnHorizBoostCharge.AddListener(() => horizBoostChargePending = true);
        this.mi = mi;
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
                    mi.currentSpeedHoriz,
                    -mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirReverseSensitivityX,
                    movementSettings.AirReverseGravityX);
            if (horizVel < 0) horizVel = 0;
        }
        else if (mi.currentSpeedHoriz > movementSettings.MaxSpeed)
        {
            horizVel =
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirSensitivityX,
                    movementSettings.AirGravityXOverTopSpeed);
        }
        else
        {
            horizVel =
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
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
            return new Run(mm, mii, mi, movementSettings);
        }
        if (divePending)
        {
            return new Dive(mm, mii, mi, movementSettings);
        }
        if (horizBoostChargePending)
        {
            return new HorizAirBoostCharge(mm, mii, mi, vertVel, movementSettings);
        }
        if (vertBoostChargePending)
        {
            return new VertAirBoostCharge(mm, mii, mi, vertVel, movementSettings);
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
}
