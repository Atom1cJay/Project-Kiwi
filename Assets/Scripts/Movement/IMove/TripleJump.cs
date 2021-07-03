using System;
using UnityEngine;
using System.Collections;

public class TripleJump : AMove
{
    float gravity;
    float horizVel;
    float vertVel;
    bool divePending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;
    bool jumpCancelled;
    bool groundPoundPending;
    bool hasInitiatedAirReverse; // permanent once activated todo change?

    /// <summary>
    /// Constructs a TripleJump, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public TripleJump(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;
        gravity = movementSettings.TjInitGravity;
        vertVel = movementSettings.TjInitJumpVel;
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnVertBoostCharge.AddListener(() => vertBoostChargePending = true);
        mii.OnHorizBoostCharge.AddListener(() => horizBoostChargePending = true);
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mii.OnJumpCancelled.AddListener(() =>
        {
            jumpCancelled = true;
            vertVel *= movementSettings.TjVelocityMultiplier;
        });
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
        horizVel = Math.Min(horizVel, mi.GetEffectiveSpeed());
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
        }
        else
        {
            horizVel =
                InputUtils.SmoothedInput(
                    horizVel,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.TjAirSensitivityX,
                    movementSettings.TjInputGravityX);
        }
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
        if (mii.PressingBoost())
        {
            return movementSettings.GroundBoostRotationSpeed;
        }
        if (horizVel < movementSettings.InstantRotationSpeed && !hasInitiatedAirReverse)
        {
            return float.MaxValue;
        }
        return hasInitiatedAirReverse ? 0 : movementSettings.AirRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (mi.TouchingGround())
        {
            if (horizVel < 0) horizVel = 0;
            return new Run(mii, mi, movementSettings, horizVel);
        }
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
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

    public override bool AdjustToSlope()
    {
        return false;
    }
}
