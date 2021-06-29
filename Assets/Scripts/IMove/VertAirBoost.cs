using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertAirBoost : AMove
{
    float vertVel;
    float horizVel;
    readonly MovementInputInfo mii;
    readonly MovementInfo mi;
    bool divePending;

    public VertAirBoost(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, float propCharged, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = movementSettings.VertBoostMinVel + (propCharged * (movementSettings.VertBoostMaxVel - movementSettings.VertBoostMinVel));
        this.mii = mii;
        mii.OnDiveInput.AddListener(() => divePending = true);
        this.mi = mi;
    }

    public override void AdvanceTime()
    {
        // Vertical
        vertVel -= movementSettings.VertBoostGravity * Time.deltaTime;
        // Horizontal
        horizVel = InputUtils.SmoothedInput(
            mi.currentSpeedHoriz,
            mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
            movementSettings.AirSensitivityX,
            movementSettings.AirGravityX);
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
        return movementSettings.AirRotationSpeed;
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
}
