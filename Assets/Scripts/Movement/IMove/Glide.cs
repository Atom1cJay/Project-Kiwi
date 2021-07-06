using System;
using UnityEngine;

public class Glide : AMove
{
    bool groundPoundPending; // Temp
    float vertVel;
    float horizVel;
    float tilt; // Angle forward/backward. Negative = forward
    float rotSpeed;

    public Glide(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel, float vertVel) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
    }

    public override void AdvanceTime()
    {
        Debug.Log("Left: " + mii.GetHorizontalInput() + ", " + "Right: " + mii.GetCameraInput());
        // Tilt
        tilt += movementSettings.GlideTiltSensitivity * mii.GetHorizontalInput().y * Time.deltaTime;
        // Horizontal
        horizVel += movementSettings.GliderWeight * Mathf.Sin(tilt);
        // Vertical
        vertVel = -horizVel * Mathf.Tan(tilt);
        vertVel -= movementSettings.GlideAirLoss;
        // Rotation
        rotSpeed += mii.GetHorizontalInput().x * movementSettings.GlideRotationSpeedSensitivity * Time.deltaTime;
        if (rotSpeed > movementSettings.GlideRotationSpeed)
        {
            rotSpeed = movementSettings.GlideRotationSpeed;
        }
        if (rotSpeed < -movementSettings.GlideRotationSpeed)
        {
            rotSpeed = -movementSettings.GlideRotationSpeed;
        }
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
        return rotSpeed;
    }

    public override IMove GetNextMove()
    {
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
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
