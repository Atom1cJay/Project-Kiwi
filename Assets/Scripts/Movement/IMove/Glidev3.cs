using System;
using UnityEngine;

public class Glidev3 : AMove
{
    float vertVel;
    float horizVel;
    float tilt; // Forward-backward
    float rotationSpeed;

    public Glidev3(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;
    }

    public override void AdvanceTime()
    {
        tilt = InputUtils.SmoothedInput(tilt, movementSettings.GlideMaxTilt * Mathf.Deg2Rad * mii.GetRelativeHorizontalInput().y, movementSettings.GlideTiltSensitivity, movementSettings.GlideTiltSensitivity);
        horizVel = Mathf.Cos(tilt) * movementSettings.GlideMaxHorizontalSpeed;
        rotationSpeed = InputUtils.SmoothedInput(rotationSpeed, movementSettings.GlideRotationSpeed * mii.GetRelativeHorizontalInput().x, movementSettings.GlideRotationSpeedSensitivity, movementSettings.GlideRotationSpeedGravity);
        vertVel = -Mathf.Sin(tilt) * movementSettings.GlideMaxVerticalSpeed;
        if (vertVel > 0) vertVel = 0;
        vertVel -= movementSettings.GlideAirLoss;
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
        else if (mi.GetGroundDetector().Colliding())
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
