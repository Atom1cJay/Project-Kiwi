using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Represents movement in the water.
/// </summary>
public class Swim : AMove
{
    Vector2 horizVector;
    float maxSpeed;
    bool jumpPending;
    private bool receivedJumpFeedback;
    private bool receivedKnockbackFeedback;
    private Vector3 knockbackFeedbackNormal; // Only to be accessed if received knockback feedback

    /// <summary>
    /// Constructs a Swim, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVector">The horizontal velocity going into this move</param>
    public Swim(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(ms, mi, mii)
    {
        this.horizVector = horizVector;
        maxSpeed = movementSettings.SwimMaxSpeedNormal;
        if (horizVector.magnitude > maxSpeed)
        {
            this.horizVector = horizVector.normalized * maxSpeed;
        }
        mii.OnJump.AddListener(() => jumpPending = true);
        mi.onJumpAttackFeedbackReceived.AddListener(() => receivedJumpFeedback = true);
        mi.ph.onBasicHit.AddListener((Vector3 basicHitNormal) => { receivedKnockbackFeedback = true; knockbackFeedbackNormal = basicHitNormal; });
    }

    public override void AdvanceTime()
    {
        float startingMagn = Mathf.Min(horizVector.magnitude, mi.GetEffectiveSpeed().magnitude);
        horizVector = horizVector.normalized * startingMagn;
        if (mii.PressingBoost())
        {
            maxSpeed = movementSettings.SwimMaxSpeedBoosted;
            horizVector += mii.GetRelativeHorizontalInputToCamera() * movementSettings.SwimSensitivityBoosted * Time.deltaTime;
        }
        else
        {
            if (horizVector.magnitude < movementSettings.SwimMaxSpeedNormal)
            {
                maxSpeed = movementSettings.SwimMaxSpeedNormal;
            }
            horizVector += mii.GetRelativeHorizontalInputToCamera() * movementSettings.SwimSensitivityNormal * Time.deltaTime;
        }

        if (mii.GetRelativeHorizontalInputToCamera().magnitude < 0.1f)
        {
            float magn = horizVector.magnitude;
            if (magn > movementSettings.SwimMaxSpeedNormal)
            {
                magn -= movementSettings.SwimOutOfBoostGravity * Time.deltaTime;
                maxSpeed = magn;
            }
            else
            {
                magn -= movementSettings.SwimGravity * Time.deltaTime;
            }
            horizVector = horizVector.normalized * magn;
        }

        if (!mii.PressingBoost() && maxSpeed > movementSettings.SwimMaxSpeedNormal)
        {
            maxSpeed -= movementSettings.SwimOutOfBoostGravity * Time.deltaTime;
        }

        maxSpeed = Mathf.Clamp(maxSpeed, movementSettings.SwimMaxSpeedNormal, movementSettings.SwimMaxSpeedBoosted);

        if (horizVector.magnitude > maxSpeed)
        {
            horizVector = horizVector.normalized * maxSpeed;
        }
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return horizVector;
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override float GetRotationSpeed()
    {
        return movementSettings.SwimRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        // Handle Feedback Moves
        if (receivedJumpFeedback)
        {
            return new Jump(mii, mi, movementSettings, horizVector.magnitude);
        }
        if (receivedKnockbackFeedback)
        {
            return new Knockback(mii, mi, movementSettings, knockbackFeedbackNormal, horizVector);
        }
        // Handle Everything Else
        if (jumpPending)
        {
            return new Jump(mii, mi, movementSettings, horizVector.magnitude);
        }
        if (mi.TouchingGround() && !mi.TouchingWater())
        {
            if (horizVector.magnitude > 0)
            {
                return new Run(mii, mi, movementSettings, horizVector);
            }
            else
            {
                return new Idle(mii, mi, movementSettings);
            }
        }
        return this;
    }

    public override string AsString()
    {
        return "swim";
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

    public override MovementParticleInfo.MovementParticles[] GetParticlesToSpawn()
    {
        return new MovementParticleInfo.MovementParticles[] { MovementParticleInfo.Instance.Splash };
    }

    public override bool Pausable()
    {
        return true;
    }
}