using System;
using System.Collections;
using UnityEngine;

public class Glidev3 : AMove
{
    float vertVel;
    float horizVel;
    float tilt; // Forward-backward
    float rotationSpeed;
    bool objectHitPending;
    bool inControl;
    bool glideReleasePending;
    float initHorizVel;

    public Glidev3(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        initHorizVel = horizVel;
        this.horizVel = horizVel;
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", GiveControl());
        mii.OnGlide.AddListener(() => glideReleasePending = true);
    }

    public override void AdvanceTime()
    {
        if (inControl)
        {
            tilt = InputUtils.SmoothedInput(tilt, movementSettings.GlideMaxTilt * Mathf.Deg2Rad * mii.GetRelativeHorizontalInput().y, movementSettings.GlideTiltSensitivity, movementSettings.GlideTiltSensitivity);
            horizVel = Mathf.Cos(tilt) * movementSettings.GlideMaxHorizontalSpeed;
            rotationSpeed = InputUtils.SmoothedInput(rotationSpeed, movementSettings.GlideRotationSpeed * mii.GetRelativeHorizontalInput().x, movementSettings.GlideRotationSpeedSensitivity, movementSettings.GlideRotationSpeedGravity);
            vertVel = -Mathf.Sin(tilt) * movementSettings.GlideMaxVerticalSpeed;
            if (vertVel > 0) vertVel = 0;
            vertVel -= movementSettings.GlideAirLoss;
            mi.OnCharContTouchSomething.AddListener(() => objectHitPending = true);
        }
    }

    IEnumerator GiveControl()
    {
        float timePassed = 0;

        while (timePassed < movementSettings.GlideJumpTime)
        {
            tilt = (timePassed / movementSettings.GlideJumpTime) * Mathf.PI / 2;
            vertVel = Mathf.Cos(tilt) * movementSettings.GlideJumpSpeed;
            //horizVel = Mathf.Sin(tilt) * movementSettings.GlideJumpSpeed;
            //vertVel = movementSettings.GlideJumpSpeed * (1 - (timePassed / movementSettings.GlideJumpTime));
            //horizVel = initHorizVel * (1 - (timePassed / movementSettings.GlideJumpTime));
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (timePassed < movementSettings.GlideNonControlTime)
        {
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        tilt = 0;

        inControl = true;
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
        return rotationSpeed; // Change
    }

    public override IMove GetNextMove()
    {
        if (glideReleasePending)
        {
            return new Fall(mii, mi, movementSettings, horizVel, false);
        }
        if (mi.TouchingGround())
        {
            if (mii.GetHorizontalInput().magnitude <= .25f)
            {
                return new Idle(mii, mi, movementSettings);
            }
            else
            {
                return new Run(mii, mi, movementSettings, horizVel);
            }
        }
        else if (objectHitPending)
        {
            return new Fall(mii, mi, movementSettings, horizVel, false);
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

    public override bool RotationIsRelative()
    {
        return true;
    }

    public override float CameraRotateTowardsRatio()
    {
        if (!inControl)
        {
            return 0;
        }
        return movementSettings.GlideCameraAdjustRatio;
    }

    public override float CameraVerticalAutoTarget()
    {
        return movementSettings.GlideMinCamAngleX + ((tilt / (movementSettings.GlideMaxTilt * Mathf.Deg2Rad)) * (movementSettings.GlideMaxCamAngleX - movementSettings.GlideMinCamAngleX));
    }

    public override string AsString()
    {
        return "glide";
    }
}
