using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AJump : AMove
{
    protected float gravity;
    protected float vertVel;
    protected float horizVel;
    protected bool divePending;
    protected bool vertBoostChargePending;
    protected bool horizBoostChargePending;
    protected bool groundPoundPending;
    protected bool jumpCancelled;
    protected bool jumpGroundableTimerComplete;
    protected bool jumpTimeShouldBreakTJ;
    protected bool glidePending;
    protected Vector2 horizVector;

    public AJump(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float initVel, float cancelMultiplier) : base(ms, mi, mii)
    {
        horizVector = mi.GetEffectiveSpeed();
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", IncrementJumpTimer());
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitForJumpGroundableTimer());
        gravity = movementSettings.JumpInitGravity;
        vertVel = initVel;
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
                vertVel *= cancelMultiplier;
            }
        });
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
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override IMove GetNextMove()
    {
        // Handle Feedback Moves
        IMove feedbackMove = GetFeedbackMove(horizVector);
        if (feedbackMove != null)
        {
            return feedbackMove;
        }
        // Handle All Other Types of Moves
        if (PlayerSlopeHandler.ShouldSlide && mi.TouchingGround())
        {
            return new Slide(mii, mi, movementSettings, horizVector);
        }
        if (glidePending)
        {
            return new Glidev3(mii, mi, movementSettings, horizVector);
        }
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        if (mi.TouchingGround() && jumpGroundableTimerComplete && vertVel < 0)
        {
            if (horizVel < 0) horizVel = 0; // todo outdated?
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
        if (horizBoostChargePending)
        {
            return new HorizAirBoostCharge(mii, mi, movementSettings, vertVel, horizVector);
        }
        if (vertBoostChargePending && (!mi.InAntiBoostZone() || vertVel > 0))
        {
            return new VertAirBoostCharge(mii, mi, movementSettings, vertVel, horizVector);
        }

        return this;
    }

    public override RotationInfo GetRotationInfo()
    {
        if (divePending || horizBoostChargePending || vertBoostChargePending)
        {
            return new RotationInfo(float.MaxValue, false);
        }
        return new RotationInfo(movementSettings.AirRotationSpeed, true);
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

    public override Attack[] GetAttack()
    {
        return new Attack[] { movementSettings.JumpAttack };
    }

    protected void AdvanceTimeVertical(float gravIncCanc, float gravIncUncanc, float gravMaxUncanc, float gravMaxCanc, float minVel)
    {
        gravity += jumpCancelled ? gravIncCanc * Time.deltaTime : gravIncUncanc * Time.deltaTime;
        if (gravity > gravMaxUncanc && !jumpCancelled)
            gravity = gravMaxUncanc;
        else if (gravity > gravMaxCanc && jumpCancelled)
            gravity = gravMaxCanc;
        vertVel -= gravity * Time.deltaTime;
        if (vertVel < minVel)
        {
            vertVel = minVel;
        }
    }

    protected void AdvanceTimeHorizontal(float sensReverse, float sens, float adjustSens, float overMaxDecRate, float gravityX)
    {
        float startingMagn = Mathf.Min(horizVector.magnitude, mi.GetEffectiveSpeed().magnitude);
        horizVector = horizVector.normalized * startingMagn;
        bool inReverse = (horizVector + mii.GetRelativeHorizontalInputToCamera()).magnitude < horizVector.magnitude;
        // Choose which type of sensitivity to employ
        if (horizVector.magnitude < movementSettings.MaxSpeed)
        {
            horizVector += inReverse ?
                mii.GetRelativeHorizontalInputToCamera() * sensReverse * Time.deltaTime
                :
                mii.GetRelativeHorizontalInputToCamera() * sens * Time.deltaTime;
        }
        else if (horizVector.magnitude >= movementSettings.MaxSpeed)
        {
            float magn = horizVector.magnitude;
            horizVector += inReverse ?
                mii.GetRelativeHorizontalInputToCamera() * sensReverse * Time.deltaTime
                :
                mii.GetRelativeHorizontalInputToCamera() * adjustSens * Time.deltaTime;
            if (horizVector.magnitude < magn)
            {
                magn = horizVector.magnitude;
            }
            horizVector = horizVector.normalized * (magn - (overMaxDecRate * Time.deltaTime));
        }
        // Come to a stop
        if (mii.GetRelativeHorizontalInputToCamera().magnitude < 0.1f)
        {
            float magn = horizVector.magnitude;
            magn -= gravityX * Time.deltaTime;
            horizVector = horizVector.normalized * magn;
        }
        // Limit Speed
        if (horizVector.magnitude > movementSettings.MaxSpeedAbsolute)
        {
            horizVector = horizVector.normalized * movementSettings.MaxSpeedAbsolute;
        }
    }
}
