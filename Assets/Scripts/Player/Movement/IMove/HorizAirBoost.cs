using System;
using UnityEngine;
using System.Collections;

public class HorizAirBoost : AMove
{
    float gravity;
    float vertVel;
    float horizVel;

    //bool objectHitPending;

    /// <summary>
    /// Constructs a HorizAirBoost, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    public HorizAirBoost(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float propCharged, float vertVel, float horizVel) : base(ms, mi, mii)
    {
        this.vertVel = vertVel / 6;
        gravity = movementSettings.HorizBoostMinGravity;
        this.horizVel = movementSettings.HorizBoostMinSpeedX + (propCharged * (movementSettings.HorizBoostMaxSpeedX - movementSettings.HorizBoostMinSpeedX));
        //mi.OnCharContTouchSomething.AddListener(() => objectHitPending = true);
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

    public override RotationInfo GetRotationInfo()
    {
        float speed = mii.AirReverseInput() ? 0 : movementSettings.HorizBoostRotation;
        return new RotationInfo(speed, true);
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
        if (mi.TouchingGround())
        {
            return new BoostSlide(mii, mi, movementSettings, horizVel, false);
        }
        if (/*objectHitPending*/mi.BonkDetectorTouching())
        {
            Vector3 knockbackDir = new Vector3(-ForwardMovement(horizVel).x, 0, -ForwardMovement(horizVel).y);
            return new Knockback(mii, mi, movementSettings, knockbackDir, ForwardMovement(horizVel));
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

    public override Attack[] GetAttack()
    {
        return new Attack[] { movementSettings.HorizBoostAttack, movementSettings.JumpAttack };
    }

    public override MovementParticleInfo.MovementParticles[] GetParticlesToSpawn()
    {
        return new MovementParticleInfo.MovementParticles[] { MovementParticleInfo.Instance.HorizBoost };
    }
}
