using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : AMove
{
    float vertVel;
    MovementInputInfo mii;
    MovementInfo mi;
    bool divePending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;

    public Fall(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = 0;
        this.mii = mii;
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnVertBoostCharge.AddListener(() => vertBoostChargePending = true);
        mii.OnHorizBoostCharge.AddListener(() => horizBoostChargePending = true);
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        float toReturn;

        if (mm.IsAirReversing())
        {
            toReturn =
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
                    -mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirReverseSensitivityX,
                    movementSettings.AirReverseGravityX);
            if (toReturn < 0) toReturn = 0;
        }
        else if (mi.currentSpeedHoriz > movementSettings.MaxSpeed)
        {
            toReturn =
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirSensitivityX,
                    movementSettings.AirGravityXOverTopSpeed);
        }
        else
        {
            toReturn =
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirSensitivityX,
                    movementSettings.AirGravityX);
        }

        return toReturn;
    }

    public override float GetVertSpeedThisFrame()
    {
        vertVel -= movementSettings.DefaultGravity * Time.deltaTime;
        return vertVel;
    }

    public override float GetRotationThisFrame()
    {
        return mm.IsAirReversing() ? 0 : movementSettings.AirRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (mi.touchingGround())
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

    public override string asString()
    {
        return "fall";
    }
}
