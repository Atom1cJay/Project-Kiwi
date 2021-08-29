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
    bool glidePending;
    bool swimPending;
    Vector2 horizVector;

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
        // TODO inertia? (same for other jumps)
        horizVector = ForwardMovement(horizVel);
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", IncrementJumpTimer());
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitForJumpGroundableTimer());
        gravity = movementSettings.JumpInitGravity;
        vertVel = movementSettings.JumpInitVel;
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnVertBoostCharge.AddListener(() => vertBoostChargePending = true);
        mii.OnHorizBoostCharge.AddListener(() => horizBoostChargePending = true);
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mii.OnGlide.AddListener(() => glidePending = true);
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
        mii.OnJumpCancelled.AddListener(() =>
        {
            if (vertVel > movementSettings.JumpVelocityOfNoReturn)
            {
                jumpCancelled = true;
                vertVel *= movementSettings.JumpVelMultiplierAtCancel;
            }
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
        if (vertVel < movementSettings.JumpMinVel)
        {
            vertVel = movementSettings.JumpMinVel;
        }
        // Horizontal
        float startingMagn = Mathf.Min(horizVector.magnitude, mi.GetEffectiveSpeed().magnitude);
        horizVector = horizVector.normalized * startingMagn;
        bool inReverse = (horizVector + mii.GetRelativeHorizontalInputToCamera()).magnitude < horizVector.magnitude;
        // Choose which type of sensitivity to employ
        if (horizVector.magnitude < movementSettings.MaxSpeed)
        {
            horizVector += inReverse ?
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpSensitivityReverseX * Time.deltaTime
                :
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpSensitivityX * Time.deltaTime;
        }
        else if (horizVector.magnitude >= movementSettings.MaxSpeed)
        {
            horizVector += inReverse ?
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpSensitivityReverseX * Time.deltaTime
                :
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpAdjustSensitivityX * Time.deltaTime;
        }
        // Don't let above the magnitude limit
        if (!mii.PressingBoost() && horizVector.magnitude > movementSettings.MaxSpeed)
        {
            horizVector = horizVector.normalized * movementSettings.MaxSpeed;
        }
        if (mii.PressingBoost() && horizVector.magnitude > movementSettings.GroundBoostMaxSpeedX)
        {
            horizVector = horizVector.normalized * movementSettings.GroundBoostMaxSpeedX;
        }
        // Come to a stop
        if (mii.GetRelativeHorizontalInputToCamera().magnitude < 0.1f)
        {
            float magn = horizVector.magnitude;
            magn -= movementSettings.JumpGravityX * Time.deltaTime;
            horizVector = horizVector.normalized * magn;
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

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return horizVector;
        //return ForwardMovement(horizVel);
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override float GetRotationSpeed()
    {
        if (divePending || horizBoostChargePending || vertBoostChargePending)
        {
            return float.MaxValue;
        }
        return movementSettings.AirRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, horizVector);
        }
        if (PlayerSlopeHandler.BeyondMaxAngle && mi.TouchingGround())
        {
            return this;
            //return new Slide(mii, mi, movementSettings, horizVector/*ForwardMovement(horizVel)*/);
        }
        if (glidePending)
        {
            return new Glidev3(mii, mi, movementSettings, horizVector /*horizVel*/);
        }
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        if (mi.TouchingGround() && jumpGroundableTimerComplete && vertVel < 0)
        {
            if (horizVel < 0) horizVel = 0;
            return new Run(mii, mi, movementSettings, /*horizVel*/ horizVector);
        }
        if (divePending)
        {
            return new Dive(mii, mi, movementSettings);
        }
        if (horizBoostChargePending && (!mi.InAntiBoostZone() || vertVel > 0))
        {
            return new HorizAirBoostCharge(mii, mi, movementSettings, vertVel, /*horizVel*/ horizVector);
        }
        if (vertBoostChargePending && (!mi.InAntiBoostZone() || vertVel > 0))
        {
            return new VertAirBoostCharge(mii, mi, movementSettings, vertVel, /*horizVel*/ horizVector);
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
        return jumpTimeShouldBreakTJ;
    }

    public override bool AdjustToSlope()
    {
        return false;
    }

    public override Attack GetAttack()
    {
        return movementSettings.JumpAttack;
    }
}
   
