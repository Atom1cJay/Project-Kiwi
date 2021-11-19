using System;
using UnityEngine;
using System.Collections;

public class HorizAirBoost : AMove
{
    float gravity;
    float vertVel;
    float horizVel;
    bool swimPending;
    private bool receivedBasicHit;
    private Vector3 basicHitNormal;

    /// <summary>
    /// Constructs a HorizAirBoost, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    public HorizAirBoost(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float propCharged, float vertVel, float horizVel) : base(ms, mi, mii)
    {
        this.vertVel = vertVel / 4;
        gravity = movementSettings.HorizBoostMinGravity;
        this.horizVel = movementSettings.HorizBoostMinSpeedX + (propCharged * (movementSettings.HorizBoostMaxSpeedX - movementSettings.HorizBoostMinSpeedX));
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
        mi.ph.onBasicHit.AddListener((Vector3 basicHitNormal) => { receivedBasicHit = true; this.basicHitNormal = basicHitNormal; });
    }

    public override void AdvanceTime()
    {
        // Vertical
        gravity += movementSettings.HorizBoostGravityIncRate * Time.deltaTime;
        if (gravity > movementSettings.HorizBoostMaxGravity)
            gravity = movementSettings.HorizBoostMaxGravity;
        vertVel -= gravity * Time.deltaTime;
        // Horizontal
        if (!mii.AirReverseInput())
        {
            horizVel = InputUtils.SmoothedInput(
                horizVel,
                movementSettings.MaxSpeed * mii.GetHorizontalInput().magnitude,
                0, movementSettings.HorizBoostNonAirReverseGravity);
        }
        else {
        horizVel = InputUtils.SmoothedInput(
            horizVel, horizVel * mii.GetHorizontalInput().magnitude, 0, movementSettings.HorizBoostAirReverseGravity);
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
        return mii.AirReverseInput() ? 0 : movementSettings.HorizBoostRotation;
    }

    public override IMove GetNextMove()
    {
        // Handle Feedback Moves
        IMove feedbackMove = GetFeedbackMove(ForwardMovement(horizVel));
        if (feedbackMove != null)
        {
            return feedbackMove;
        }
        // Handle Everything Else
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (receivedBasicHit)
        {
            return new Knockback(mii, mi, movementSettings, basicHitNormal, ForwardMovement(horizVel));
        }
        if (mi.TouchingGround())
        {
            return new BoostSlide(mii, mi, movementSettings, horizVel, false);
        }

        return this;
    }

    public override string AsString()
    {
        return "horizairboost";
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

    public override Attack GetAttack()
    {
        return movementSettings.HorizBoostAttack;
    }

    public override MovementParticleInfo.MovementParticles GetParticlesToSpawn()
    {
        return MovementParticleInfo.Instance.HorizBoost;
    }
}
