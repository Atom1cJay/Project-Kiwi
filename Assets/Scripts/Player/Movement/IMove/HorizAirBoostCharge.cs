using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizAirBoostCharge : AMove
{
    float horizVel;
    float initVertVel;
    float vertVel;
    float timeCharging;
    readonly float maxTimeToCharge;
    bool boostReleasePending;
    bool swimPending;

    /// <summary>
    /// Constructs a HorizAirBoostCharge, initializing the objects that hold all
    /// the information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="vertVel">The vertical speed moving into this move</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public HorizAirBoostCharge(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float vertVel, Vector2 horizVector) : base(ms, mi, mii)
    {
        horizVel = GetSharedMagnitudeWithPlayerAngle(horizVector);
        //this.vertVel = (vertVel < 0) ? 0 : vertVel;
        this.vertVel = vertVel;
        initVertVel = vertVel;
        timeCharging = 0;
        maxTimeToCharge = movementSettings.HorizBoostMaxChargeTime;
        mii.OnHorizBoostRelease.AddListener(() => boostReleasePending = true);
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
    }

    public override void AdvanceTime()
    {
        timeCharging += Time.deltaTime;
        // Horizontal
        horizVel = InputUtils.SmoothedInput(
            horizVel, 0, 0, movementSettings.HorizBoostChargeGravityX);
        /*
        if (vertVel > 0)
        {
            vertVel -= movementSettings.HorizBoostChargeGravityY * Time.deltaTime;
        }
        if (vertVel < 0)
        {
            vertVel = 0;
        }
        */
        if (vertVel > 0)
        {
            vertVel -= movementSettings.HorizBoostChargeGravityYGoingUp * Time.deltaTime;
        }
        else
        {
            vertVel -= movementSettings.HorizBoostChargeGravityY * Time.deltaTime;
        }
        if (vertVel < movementSettings.HorizBoostChargeMinVelY)
        {
            vertVel = movementSettings.HorizBoostChargeMinVelY;
        }
        /*
        float vertChange = (initVertVel * (Time.deltaTime / movementSettings.HorizBoostChargeVertNeutralizeTime));
        if (Mathf.Abs(vertChange) > Mathf.Abs(vertVel))
        {
            vertChange = vertVel;
        }
        vertVel -= vertChange;
        */
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
        return movementSettings.HorizBoostChargeRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, ForwardMovement(horizVel));
        }

        if (timeCharging > maxTimeToCharge || boostReleasePending)
        {
            float propCharged = timeCharging / maxTimeToCharge;
            return new HorizAirBoost(mii, mi, movementSettings, propCharged, vertVel, horizVel);
        }
        else
        {
            return this;
        }
    }

    public override string AsString()
    {
        return "horizairboostcharge";
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
