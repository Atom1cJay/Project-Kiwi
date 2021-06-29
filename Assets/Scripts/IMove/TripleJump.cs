using System;
using UnityEngine;
using System.Collections;

public class TripleJump : AMove
{
    float gravity;
    float horizVel;
    float vertVel;
    readonly MovementInputInfo mii;
    readonly MovementInfo mi;
    bool divePending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;
    bool jumpCancelled;
    bool hasInitiatedAirReverse; // permanent once activated todo change?

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

    public override void AdvanceTime()
    {
        // Vertical
        if (jumpCancelled)
            gravity += movementSettings.TjGravityIncRateAtCancel * Time.deltaTime;
        else
            gravity += movementSettings.TjGravityIncRate * Time.deltaTime;

        if (gravity > movementSettings.TjUncancelledMaxGravity && !jumpCancelled)
            gravity = movementSettings.TjUncancelledMaxGravity;
        else if (gravity > movementSettings.TjCancelledMaxGravity && jumpCancelled)
            gravity = movementSettings.TjCancelledMaxGravity;
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
        else
        {
            horizVel =
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.TjAirSensitivityX,
                    movementSettings.TjInputGravityX);
        }
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
        return "triplejump";
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
