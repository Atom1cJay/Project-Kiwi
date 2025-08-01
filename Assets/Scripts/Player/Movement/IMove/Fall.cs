using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : AMove
{
    Vector2 horizVector;
    float vertVel;
    bool divePending;
    bool vertBoostChargePending;
    bool horizBoostChargePending;
    bool jumpPending;
    bool groundPoundPending;
    bool glidePending;
    float coyoteTime;

    /// <summary>
    /// Constructs a Fall, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVel">The horizontal speed moving into this move</param>
    public Fall(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector, bool giveCoyoteTime) : base(ms, mi, mii)
    {
        this.horizVector = horizVector;
        vertVel = 0;
        coyoteTime = giveCoyoteTime ? movementSettings.CoyoteTime : 0;
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", AllowCoyoteTime());
        mii.OnDiveInput.AddListener(() => divePending = true);
        mii.OnVertBoostCharge.AddListener(() => vertBoostChargePending = true);
        mii.OnPushPress.AddListener(() => horizBoostChargePending = true);
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mii.OnGlide.AddListener(() => glidePending = true);
        mii.OnJump.AddListener(() => jumpPending = true);
    }

    IEnumerator AllowCoyoteTime()
    {
        while (coyoteTime > 0)
        {
            coyoteTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public override void AdvanceTime()
    {
        // Horizontal
        float startingMagn = Math.Min(horizVector.magnitude, mi.GetEffectiveSpeed().magnitude);
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
            float magn = horizVector.magnitude;
            horizVector += inReverse ?
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpSensitivityReverseX * Time.deltaTime
                :
                mii.GetRelativeHorizontalInputToCamera() * movementSettings.JumpAdjustSensitivityX * Time.deltaTime;
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
        // Vertical
        vertVel -= movementSettings.DefaultGravity * Time.deltaTime;
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
        return new RotationInfo(movementSettings.AirRotationSpeed, false);
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
        if (glidePending)
        {
            return new Glidev3(mii, mi, movementSettings, horizVector);
        }
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        if (PlayerSlopeHandler.ShouldSlide)
        {
            return new Slide(mii, mi, movementSettings, horizVector);
        }
        if (mi.TouchingGround() && !PlayerSlopeHandler.ShouldSlide && horizVector.magnitude > 0)
        {
            return new Run(mii, mi, movementSettings, horizVector, FromStatus.FromAir);
        }
        if (mi.TouchingGround() && !PlayerSlopeHandler.ShouldSlide && horizVector.magnitude == 0)
        {
            return new Idle(mii, mi, movementSettings);
        }
        if (jumpPending && coyoteTime > 0)
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
        return "fall";
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
        return true;
    }

    public override Attack[] GetAttack()
    {
        return new Attack[] { movementSettings.JumpAttack };
    }
}
