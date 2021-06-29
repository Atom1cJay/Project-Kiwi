using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertAirBoostCharge : AMove
{
    float vertVel;
    float horizVel;
    float timeActive;
    readonly float maxTimeActive;
    readonly MovementInputInfo mii;
    readonly MovementInfo mi;
    bool boostReleasePending;

    public VertAirBoostCharge(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, float prevVertVel, MovementSettingsSO ms) : base(mm, ms)
    {
        vertVel = (prevVertVel < 0) ? 0 : prevVertVel;
        timeActive = 0;
        maxTimeActive = movementSettings.VertBoostMaxChargeTime;
        this.mii = mii;
        mii.OnVertBoostRelease.AddListener(() => boostReleasePending = true);
        this.mi = mi;
    }

    public override void AdvanceTime()
    {
        // Meta
        timeActive += Time.deltaTime;
        // Vertical
        float gravityType = (vertVel > 0) ?
            movementSettings.DefaultGravity : movementSettings.HorizBoostChargeGravity;
        vertVel -= gravityType * Time.fixedDeltaTime;
        // Horizontal
        horizVel = InputUtils.SmoothedInput(
            mi.currentSpeedHoriz, 0, 0, movementSettings.VertBoostChargeGravityX);
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
        return 0;
    }

    public override IMove GetNextMove()
    {
        if (timeActive > maxTimeActive || boostReleasePending)
        {
            float propCharged = Mathf.Clamp01(timeActive / maxTimeActive);
            return new VertAirBoost(mm, mii, mi, propCharged, movementSettings);
        }
        return this;
    }

    public override string AsString()
    {
        return "vertairboostcharge";
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
