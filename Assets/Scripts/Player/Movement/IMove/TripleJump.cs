﻿using System;
using UnityEngine;
using System.Collections;

public class TripleJump : AMove
{
    float gravity;
    Vector2 horizVector;
    float vertVel;
    bool divePending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;
    bool jumpCancelled;
    bool jumpGroundableTimerComplete;
    bool groundPoundPending;
    bool glidePending;

    /// <summary>
    /// Constructs a TripleJump, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public TripleJump(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(ms, mi, mii)
    {
        this.horizVector = mi.GetEffectiveSpeed();
        //this.horizVector = horizVector;
        gravity = movementSettings.TjInitGravity;
        vertVel = movementSettings.TjInitJumpVel;
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnVertBoostCharge.AddListener(() => vertBoostChargePending = true);
        mii.OnPushPress.AddListener(() => horizBoostChargePending = true);
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mii.OnGlide.AddListener(() => glidePending = true);
        mii.OnJumpCancelled.AddListener(() =>
        {
            if (vertVel > movementSettings.JumpVelocityOfNoReturn)
            {
                jumpCancelled = true;
                vertVel *= movementSettings.JumpVelMultiplierAtCancel;
            }
        });
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitForJumpGroundableTimer());
    }

    // This can be run as a coroutine by using MonobehaviorUtils
    private IEnumerator WaitForJumpGroundableTimer()
    {
        yield return new WaitForSeconds(movementSettings.JumpGroundableTimer);
        jumpGroundableTimerComplete = true;
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
        float startingMagn = Math.Min(horizVector.magnitude, mi.GetEffectiveSpeed().magnitude);
        horizVector = horizVector.normalized * startingMagn;
        // Choose which type of sensitivity to employ
        if (horizVector.magnitude < movementSettings.MaxSpeed)
        {
            horizVector += mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpSensitivityX * Time.deltaTime;
        }
        else if (horizVector.magnitude >= movementSettings.MaxSpeed)
        {
            bool inReverse = (horizVector + mii.GetRelativeHorizontalInputToCamera()).magnitude < horizVector.magnitude;
            float magn = horizVector.magnitude;
            //float propToAbsoluteMax = (magn - movementSettings.MaxSpeed) / (movementSettings.MaxSpeedAbsolute - movementSettings.MaxSpeed);
            // In case the jump is getting adjusted, make sure the sensitivity is appropriate to the speed
            //float jumpAdjustSensitivity = Mathf.Lerp(movementSettings.JumpAdjustSensitivityX, movementSettings.JumpAdjustSensitivityMaxSpeedX, propToAbsoluteMax);
            horizVector += inReverse ?
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpSensitivityReverseX * Time.deltaTime
                :
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpAdjustSensitivityX * Time.deltaTime;
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
        if (horizVector.magnitude > movementSettings.MaxSpeedAbsolute)
        {
            horizVector = horizVector.normalized * movementSettings.MaxSpeedAbsolute;
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
        if (divePending || horizBoostChargePending || vertBoostChargePending)
        {
            return new RotationInfo(float.MaxValue, false);
        }
        return new RotationInfo(movementSettings.AirRotationSpeed, true);
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
        if (PlayerSlopeHandler.ShouldSlide)
        {
            return new Slide(mii, mi, movementSettings, horizVector);
        }
        if (mi.TouchingGround() && jumpGroundableTimerComplete)
        {
            if (horizVector.magnitude == 0)
            {
                return new Idle(mii, mi, movementSettings, FromStatus.FromAir);
            }
            return new Run(mii, mi, movementSettings, horizVector, FromStatus.FromAir);
        }
        if (glidePending)
        {
            return new Glidev3(mii, mi, movementSettings, horizVector);
        }
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        if (divePending)
        {
            return new Dive(mii, mi, movementSettings);
        }
        if (horizBoostChargePending && (!mi.InAntiBoostZone() || vertVel > 0))
        {
            return new HorizAirBoostCharge(mii, mi, movementSettings, vertVel, horizVector);
        }
        if (vertBoostChargePending && (!mi.InAntiBoostZone() || vertVel > 0))
        {
            return new VertAirBoostCharge(mii, mi, movementSettings, vertVel, horizVector);
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

    public override Attack[] GetAttack()
    {
        return new Attack[] { movementSettings.JumpAttack };
    }
}
