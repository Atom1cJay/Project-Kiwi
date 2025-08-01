using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertAirBoost : AMove
{
    float vertVel;
    Vector2 horizVector;
    bool divePending;
    bool gpPending;
    bool spawnedParticlesFirstFrame;

    /// <summary>
    /// Constructs a VertAirBoost, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public VertAirBoost(MovementInputInfo mii, MovementInfo mi, float propCharged, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        if (horizVel < 0)
        {
            horizVel = 0;
        }
        this.horizVector = ForwardMovement(horizVel);
        vertVel = movementSettings.VertBoostMinLaunchVel + (propCharged * (movementSettings.VertBoostMaxLaunchVel - movementSettings.VertBoostMinLaunchVel));
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnGroundPound.AddListener(() => gpPending = true);
    }

    public override void AdvanceTime()
    {
        // Vertical
        vertVel -= movementSettings.VertBoostGravity * Time.deltaTime;
        if (vertVel < movementSettings.VertBoostMinGeneralVelY)
        {
            vertVel = movementSettings.VertBoostMinGeneralVelY;
        }
        // Horizontal
        float startingMagn = Mathf.Min(horizVector.magnitude, mi.GetEffectiveSpeed().magnitude);
        horizVector = horizVector.normalized * startingMagn;
        bool inReverse = (horizVector + mii.GetRelativeHorizontalInputToCamera()).magnitude < horizVector.magnitude;
        // Choose which type of sensitivity to employ
        if (horizVector.magnitude < movementSettings.VertBoostMaxSpeedX)
        {
            horizVector += inReverse ?
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.VertBoostGravityX * Time.deltaTime
                :
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.VertBoostSensitivityX * Time.deltaTime;
        }
        else
        {
            float magn = horizVector.magnitude;
            float propToAbsoluteMax = (magn - movementSettings.MaxSpeed) / (movementSettings.MaxSpeedAbsolute - movementSettings.MaxSpeed);
            // In case the jump is getting adjusted, make sure the sensitivity is appropriate to the speed
            horizVector += inReverse ?
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.VertBoostGravityX * Time.deltaTime
                :
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.VertBoostAdjustSensitivityX * Time.deltaTime;
            if (horizVector.magnitude < magn)
            {
                magn = horizVector.magnitude;
            }
            horizVector = horizVector.normalized * (magn - (movementSettings.JumpSpeedDecRateOverMaxSpeed * Time.deltaTime));
        }
        // Come to a stop
        if (mii.GetRelativeHorizontalInputToCamera().magnitude < 0.1f)
        {
            float magn = horizVector.magnitude;
            magn -= movementSettings.JumpGravityX * Time.deltaTime;
            horizVector = horizVector.normalized * magn;
        }
        // Limit Speed
        if (horizVector.magnitude > movementSettings.VertBoostMaxSpeedX && vertVel > 0)
        {
            horizVector = horizVector.normalized * movementSettings.VertBoostMaxSpeedX;
        }
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return horizVector;
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override RotationInfo GetRotationInfo()
    {
        if (divePending)
        {
            return new RotationInfo(float.MaxValue, false);
        }
        return new RotationInfo(movementSettings.VertBoostRotationSpeed, false);
    }

    public override IMove GetNextMove()
    {
        // Handle Feedback Moves
        IMove feedbackMove = GetFeedbackMove(horizVector);
        if (feedbackMove != null)
        {
            return feedbackMove;
        }
        // Handle Everything Else
        if (mi.TouchingGround() && vertVel < 0)
        {
            if (horizVector.magnitude == 0)
            {
                return new Idle(mii, mi, movementSettings, FromStatus.FromAir);
            }
            return new Run(mii, mi, movementSettings, horizVector, FromStatus.FromAir);
        }
        if (divePending)
        {
            return new Dive(mii, mi, movementSettings);
        }
        if (gpPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }

        return this;
    }

    public override string AsString()
    {
        return "vertairboost";
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
        return new Attack[] { movementSettings.VertBoostAttack, movementSettings.JumpAttack };
    }

    public override MovementParticleInfo.MovementParticles[] GetParticlesToSpawn()
    {
        if (spawnedParticlesFirstFrame)
        {
            return null;
        }
        spawnedParticlesFirstFrame = true;
        return new MovementParticleInfo.MovementParticles[] { MovementParticleInfo.Instance.VertBoost };
    }
}
