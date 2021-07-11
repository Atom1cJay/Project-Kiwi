using System;
using UnityEngine;

public class Glidev2 : AMove
{
    bool groundPoundPending; // Temp
    bool glidePending; // Temp
    float vertVel;
    float horizVelZ;
    float horizVelX;
    float tilt; // Angle forward/backward. Positive = forward
    float roll; // Angle left/right. Positive = right

    public Glidev2(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel, float vertVel) : base(ms, mi, mii)
    {
        horizVelZ = horizVel;
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mii.OnGlide.AddListener(() => glidePending = true);
        tilt = 10 * Mathf.Deg2Rad;
    }

    public override void AdvanceTime()
    {
        Debug.Log(tilt * Mathf.Rad2Deg);
        tilt = InputUtils.SmoothedInput(tilt, mii.GetRelativeHorizontalInput().y * movementSettings.GlideMaxTilt * Mathf.Deg2Rad, movementSettings.GlideTiltSensitivity, movementSettings.GlideTiltSensitivity);
        tilt = Mathf.Clamp(tilt, -movementSettings.GlideMaxTilt * Mathf.Deg2Rad, movementSettings.GlideMaxTilt * Mathf.Deg2Rad);
        roll = InputUtils.SmoothedInput(roll, mii.GetRelativeHorizontalInput().x * movementSettings.GlideMaxRoll * Mathf.Deg2Rad, movementSettings.GlideTiltSensitivity, movementSettings.GlideTiltSensitivity);
        roll = Mathf.Clamp(roll, -movementSettings.GlideMaxRoll * Mathf.Deg2Rad, movementSettings.GlideMaxRoll * Mathf.Deg2Rad);
        // TODO change max tilt to max roll in both above lines
        horizVelZ += movementSettings.GlideWeight * Mathf.Sin(tilt) * Time.deltaTime;
        horizVelX += movementSettings.GlideWeight * Mathf.Sin(roll) * Time.deltaTime;
        vertVel += horizVelZ * -Mathf.Tan(tilt) * Time.deltaTime;
        vertVel += horizVelX * -Mathf.Tan(roll) * Time.deltaTime;
        vertVel -= movementSettings.GlideAirLoss * Time.deltaTime;
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return ForwardMovement(horizVelZ) - SideMovement(horizVelX);
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override float GetRotationSpeed()
    {
        return mii.GetRelativeHorizontalInput().x * movementSettings.GlideRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        else if (mi.TouchingGround())
        {
            if (mii.GetHorizontalInput().magnitude <= .25f)
            {
                return new Idle(mii, mi, movementSettings);
            }
            else
            {
                return new Run(mii, mi, movementSettings, horizVelZ);
            }
        }
        else if (glidePending)
        {
            return new Fall(mii, mi, movementSettings, horizVelZ, false);
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

    public override string AsString()
    {
        return "glide";
    }
}
