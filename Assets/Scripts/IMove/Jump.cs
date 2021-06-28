using System;
using System.Collections;
using UnityEngine;

public class Jump : AMove
{
    float gravity;
    float vertVel;
    MovementInputInfo mii;
    MovementInfo mi;
    bool divePending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;
    bool jumpCancelled;
    bool jumpGroundableTimerComplete;

    public Jump(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitForJumpGroundableTimer());
        gravity = movementSettings.JumpInitGravity;
        vertVel = movementSettings.JumpInitVel;
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

    // This can be run as a coroutine by using MonobehaviorUtils
    private IEnumerator WaitForJumpGroundableTimer()
    {
        yield return new WaitForSeconds(movementSettings.JumpGroundableTimer);
        jumpGroundableTimerComplete = true;
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
        // Decide Gravity
        if (jumpCancelled)
            gravity += movementSettings.JumpCancelledGravityIncrease * Time.deltaTime;
        else
            gravity += movementSettings.JumpUncancelledGravityIncrease * Time.deltaTime;

        if (gravity > movementSettings.JumpMaxUncancelledGravity && !jumpCancelled)
            gravity = movementSettings.JumpMaxUncancelledGravity;
        else if (gravity > movementSettings.JumpMaxCancelledGravity && jumpCancelled)
            gravity = movementSettings.JumpMaxCancelledGravity;

        // Effect Gravity
        vertVel -= gravity * Time.deltaTime;
        return vertVel;
    }

    public override float GetRotationThisFrame()
    {
        return mm.IsAirReversing() ? 0 : movementSettings.AirRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (mi.touchingGround() && jumpGroundableTimerComplete)
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
        return "jump";
    }
}
   
