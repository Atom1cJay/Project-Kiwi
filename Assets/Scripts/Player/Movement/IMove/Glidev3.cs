using System;
using System.Collections;
using UnityEngine;

public class Glidev3 : AMove
{
    bool jumpOver;
    float vertVel;
    Vector2 horizVector;
    bool objectHitPending;
    bool glideReleasePending;
    bool groundPoundPending;
    bool swimPending;
    float maxSpeed;

    public Glidev3(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(ms, mi, mii)
    {
        this.horizVector = horizVector;
        maxSpeed = movementSettings.GlideMaxHorizontalSpeed;
        horizVector = horizVector.normalized * maxSpeed;
        if (this.horizVector.magnitude > movementSettings.GlideMaxHorizontalSpeed)
        {
            this.horizVector = this.horizVector.normalized * movementSettings.GlideMaxHorizontalSpeed;
        }
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", GiveControl());
        mii.OnGlide.AddListener(() => glideReleasePending = true);
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mi.OnCharContTouchSomething.AddListener(() => objectHitPending = true);
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
    }

    public override void AdvanceTime()
    {
        if (mii.PressingBoost())
        {
            maxSpeed = movementSettings.GlideMaxHorizontalSpeedBoosted;
            horizVector += mii.GetRelativeHorizontalInputToCamera() * movementSettings.GlideXSensitivityBoosting * Time.deltaTime;
        }
        else
        {
            if (horizVector.magnitude < movementSettings.GlideMaxHorizontalSpeed)
            {
                maxSpeed = movementSettings.GlideMaxHorizontalSpeed;
            }
            horizVector += mii.GetRelativeHorizontalInputToCamera() * movementSettings.GlideXSensitivity * Time.deltaTime;
        }

        if (mii.GetRelativeHorizontalInputToCamera().magnitude < 0.1f)
        {
            float magn = horizVector.magnitude;
            if (magn > movementSettings.GlideMaxHorizontalSpeed)
            {
                magn -= movementSettings.GlideXGravityOutOfBoost * Time.deltaTime;
                maxSpeed = magn;
            }
            else
            {
                magn -= movementSettings.GlideXGravity * Time.deltaTime;
            }
            horizVector = horizVector.normalized * magn;
        }

        if (!mii.PressingBoost() && maxSpeed > movementSettings.GlideMaxHorizontalSpeed)
        {
            maxSpeed -= movementSettings.GlideXGravityOutOfBoost * Time.deltaTime;
        }

        maxSpeed = Mathf.Clamp(maxSpeed, movementSettings.GlideMaxHorizontalSpeed, movementSettings.GlideMaxHorizontalSpeedBoosted);

        if (horizVector.magnitude > maxSpeed)
        {
            horizVector = horizVector.normalized * maxSpeed;
        }

        if (jumpOver)
        {
            // Goes down faster the faster you are horizontally
            vertVel = -movementSettings.GlideAirLoss * (maxSpeed / movementSettings.GlideMaxHorizontalSpeed);
        }
    }

    IEnumerator GiveControl()
    {
        float timePassed = 0;
        float tilt;

        while (timePassed < movementSettings.GlideJumpTime)
        {
            tilt = (timePassed / movementSettings.GlideJumpTime) * Mathf.PI / 2;
            vertVel = -Mathf.Cos(tilt) * movementSettings.GlideJumpSpeed - movementSettings.GlideAirLoss;
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (timePassed < movementSettings.GlideNonControlTime)
        {
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        jumpOver = true;
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return horizVector;
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override float GetRotationSpeed()
    {
        return movementSettings.GlideRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, horizVector);
        }
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        if (glideReleasePending)
        {
            return new Fall(mii, mi, movementSettings, horizVector, false);
        }
        if (mi.TouchingGround())
        {
            if (mii.GetHorizontalInput().magnitude <= .25f)
            {
                return new Idle(mii, mi, movementSettings);
            }
            else
            {
                return new Run(mii, mi, movementSettings, horizVector);
            }
        }
        else if (objectHitPending)
        {
            return new Fall(mii, mi, movementSettings, horizVector, false);
        }
        return this;
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

    public override string AsString()
    {
        return "glide";
    }
}
