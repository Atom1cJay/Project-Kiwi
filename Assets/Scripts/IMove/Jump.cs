using System;
using System.Collections;
using UnityEngine;

public class Jump : AMove
{
    float gravity;
    float vertVel;
    float horizVel;
    readonly MovementInputInfo mii;
    readonly MovementInfo mi;
    bool divePending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;
    bool jumpCancelled;
    bool jumpGroundableTimerComplete;
    bool jumpTimeShouldBreakTJ;
    bool hasInitiatedAirReverse; // permanent once activated todo change?

    public Jump(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", IncrementJumpTimer());
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

    public override void AdvanceTime()
    {
        // Vertical
        if (jumpCancelled)
            gravity += movementSettings.JumpCancelledGravityIncrease * Time.deltaTime;
        else
            gravity += movementSettings.JumpUncancelledGravityIncrease * Time.deltaTime;

        if (gravity > movementSettings.JumpMaxUncancelledGravity && !jumpCancelled)
            gravity = movementSettings.JumpMaxUncancelledGravity;
        else if (gravity > movementSettings.JumpMaxCancelledGravity && jumpCancelled)
            gravity = movementSettings.JumpMaxCancelledGravity;
        vertVel -= gravity * Time.deltaTime;
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
    }

    IEnumerator IncrementJumpTimer()
    {
        yield return new WaitForSeconds(movementSettings.TjMaxJumpTime);
        jumpTimeShouldBreakTJ = true;
    }

    // This can be run as a coroutine by using MonobehaviorUtils
    private IEnumerator WaitForJumpGroundableTimer()
    {
        yield return new WaitForSeconds(movementSettings.JumpGroundableTimer);
        jumpGroundableTimerComplete = true;
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
        if (mi.TouchingGround() && jumpGroundableTimerComplete)
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
        return "jump";
    }

    public override bool IncrementsTJcounter()
    {
        return true;
    }

    public override bool TJshouldBreak()
    {
        return mii.GetHorizDissonance() > movementSettings.TjMaxDissonance
            || mii.GetHorizontalInput().magnitude < movementSettings.TjMinHorizInputMagnitude
            || jumpTimeShouldBreakTJ;
    }
}
   
