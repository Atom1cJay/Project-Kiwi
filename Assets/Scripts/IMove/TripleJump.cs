using System;
using UnityEngine;
using System.Collections;

public class TripleJump : AMove
{
    float gravity;
    float vertVel;
    MovementInputInfo mii;
    MovementInfo mi;
    bool divePending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;
    bool jumpCancelled;

    public TripleJump(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        gravity = movementSettings.TjInitGravity;
        vertVel = movementSettings.TjInitJumpVel;
        this.mii = mii;
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnVertBoostCharge.AddListener(() => vertBoostChargePending = true);
        mii.OnHorizBoostCharge.AddListener(() => horizBoostChargePending = true);
        mii.OnJumpCancelled.AddListener(() =>
        {
            jumpCancelled = true;
            vertVel *= movementSettings.JumpVelMultiplierAtCancel;
        });
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
        else
        {
            toReturn =
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.TjAirSensitivityX,
                    movementSettings.TjInputGravityX);
        }

        return toReturn;
    }

    public override float GetVertSpeedThisFrame()
    {
        // Decide Gravity
        if (jumpCancelled)
            gravity += movementSettings.TjGravityIncRateAtCancel * Time.deltaTime;
        else
            gravity += movementSettings.TjGravityIncRate * Time.deltaTime;

        if (gravity > movementSettings.TjUncancelledMaxGravity && !jumpCancelled)
            gravity = movementSettings.TjUncancelledMaxGravity;
        else if (gravity > movementSettings.TjCancelledMaxGravity && jumpCancelled)
            gravity = movementSettings.TjCancelledMaxGravity;

        // Effect Gravity
        vertVel -= gravity * Time.deltaTime;
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
        return "triplejump";
    }
}
