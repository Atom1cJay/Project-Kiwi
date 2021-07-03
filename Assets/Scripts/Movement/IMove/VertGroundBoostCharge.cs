using System;
using UnityEngine;

public class VertGroundBoostCharge : AMove
{
    float timePassed;
    float horizVel;
    bool boostReleasePending;

    public VertGroundBoostCharge(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base (ms, mi, mii)
    {
        this.horizVel = horizVel;
        mii.OnVertBoostRelease.AddListener(() => boostReleasePending = true);
    }

    public override bool AdjustToSlope()
    {
        return true;
    }

    public override void AdvanceTime()
    {
        // Meta
        timePassed += Time.deltaTime;
        // Horizontal
        if (mii.PressingBoost())
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    mii.GetHorizontalInput().magnitude * movementSettings.GroundBoostMaxSpeedX,
                    movementSettings.GroundBoostSensitivityX,
                    movementSettings.GroundBoostGravityX);
        }
        else if (horizVel > movementSettings.MaxSpeed)
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.RunSensitivityX,
                    movementSettings.RunGravityX);
        }
        else
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.RunSensitivityX,
                    movementSettings.RunGravityX);
        }
    }

    public override string AsString()
    {
        return "vertgroundboostcharge";
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return ForwardMovement(horizVel);
    }

    public override IMove GetNextMove()
    {
        if (boostReleasePending || timePassed > movementSettings.VertBoostMaxChargeTime)
        {
            float propCharged = timePassed / movementSettings.VertBoostMaxChargeTime;
            if (propCharged > 1) propCharged = 1;
            return new VertAirBoost(mii, mi, propCharged, movementSettings, horizVel);
        }
        return this;
    }

    public override float GetRotationSpeed()
    {
        if (mii.PressingBoost())
        {
            return movementSettings.GroundBoostRotationSpeed;
        }
        return (horizVel < movementSettings.InstantRotationSpeed) ?
            float.MaxValue : movementSettings.GroundRotationSpeed;
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
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
