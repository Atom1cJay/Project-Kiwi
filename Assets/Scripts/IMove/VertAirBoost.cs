using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertAirBoost : AMove
{
    float vertVel;
    MovementInputInfo mii;
    MovementInfo mi;
    bool divePending;

    public VertAirBoost(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, float propCharged, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = movementSettings.VertBoostMinVel + (propCharged * (movementSettings.VertBoostMaxVel - movementSettings.VertBoostMinVel));
        this.mii = mii;
        mii.OnDiveInput.AddListener(() => divePending = true);
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return
            InputUtils.SmoothedInput(
                mi.currentSpeedHoriz,
                mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                movementSettings.AirSensitivityX,
                movementSettings.AirGravityX);
    }

    public override float GetVertSpeedThisFrame()
    {
        vertVel -= movementSettings.VertBoostGravity * Time.deltaTime;
        return vertVel;
    }

    public override float GetRotationThisFrame()
    {
        return movementSettings.AirRotationSpeed;
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

        return this;
    }

    public override string asString()
    {
        return "vertairboost";
    }

}
