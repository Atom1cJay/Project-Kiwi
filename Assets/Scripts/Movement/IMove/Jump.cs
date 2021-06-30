using System;
using System.Collections;
using UnityEngine;

public class Jump : AMove
{
    float gravity;
    float vertVel;
    float horizVel;
    bool divePending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;
    bool groundPoundPending;
    bool jumpCancelled;
    bool jumpGroundableTimerComplete;
    bool jumpTimeShouldBreakTJ;
    bool hasInitiatedAirReverse; // permanent once activated todo change?
    bool firstFrameAirReverse;

    /// <summary>
    /// Constructs a Jump, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public Jump(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", IncrementJumpTimer());
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitForJumpGroundableTimer());
        gravity = movementSettings.JumpInitGravity;
        vertVel = movementSettings.JumpInitVel;
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnVertBoostCharge.AddListener(() => vertBoostChargePending = true);
        mii.OnHorizBoostCharge.AddListener(() => horizBoostChargePending = true);
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mii.OnJumpCancelled.AddListener(() =>
        {
            jumpCancelled = true;
            vertVel *= movementSettings.JumpVelMultiplierAtCancel;
        });
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

        if (mii.PressingBoost())
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    movementSettings.GroundBoostMaxSpeedX,
                    movementSettings.GroundBoostSensitivityX,
                    movementSettings.GroundBoostGravityX);
        }
        else if (hasInitiatedAirReverse)
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
        if (horizVel < movementSettings.InstantRotationSpeed)
        {
            return float.MaxValue;
        }
        return hasInitiatedAirReverse ? 0 : movementSettings.AirRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        if (mi.TouchingGround() && jumpGroundableTimerComplete && vertVel < 0)
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

    public override bool AdjustToSlope()
    {
        return false;
    }
}
   
