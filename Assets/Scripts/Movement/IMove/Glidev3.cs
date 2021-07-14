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

    public Glidev3(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", GiveControl());
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

        while (timePassed < movementSettings.GlideNonControlTime)
        {
            float speed = (timePassed / movementSettings.GlideNonControlTime) * movementSettings.GlideMaxHorizontalSpeed;
            horizVel = speed;
            vertVel = -movementSettings.GlideAirLoss * (timePassed / movementSettings.GlideNonControlTime);
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

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
        return movementSettings.GlideCameraAdjustRatio;
    }

    public override string AsString()
    {
        return "glide";
    }
}
