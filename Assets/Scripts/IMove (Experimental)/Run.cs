using System;
using UnityEngine;

/// <summary>
/// Represents movement on the ground while jumping is not occurring.
/// </summary>
public class Run : IMove
{
    // The player's speed the previous frame
    private readonly float speedLastFrame;
    // The "max" speed during regular movement. Speed can go higher than this
    // during special periods, such as the horizontal boost move. The speed
    // should always eventually come back to this.
    private readonly float conventionalMaxSpeed;
    // The current horizontal input.
    private readonly Vector2 horizontalInput;
    private readonly float sensitivity;
    private readonly float gravity;
    private readonly float overTopSpeedGravity;

    public Run(float speedLastFrame, float conventionalMaxSpeed,
        Vector2 horizontalInput, float sensitivity,
        float gravity, float overTopSpeedGravity)
    {
        if (speedLastFrame < 0)
        {
            Debug.LogError("speedLastFrame being below 0 doesn't make sense.");
        }
        if (conventionalMaxSpeed < 0)
        {
            Debug.LogError("conv. max speed being below 0 doesn't make sense.");
        }
        if (horizontalInput.magnitude > 1)
        {
            Debug.LogError("horiz input magnitude exceeding 1 makes no sense.");
        }
        if (sensitivity < 0)
        {
            Debug.LogError("sensitivity being below 0 doesn't make sense.");
        }
        if (gravity < 0)
        {
            Debug.LogError("gravity speed being below 0 doesn't make sense.");
        }
        if (overTopSpeedGravity < 0)
        {
            Debug.LogError("gravity speed being below 0 doesn't make sense.");
        }
        this.speedLastFrame = speedLastFrame;
        this.conventionalMaxSpeed = conventionalMaxSpeed;
        this.horizontalInput = new Vector2(horizontalInput.x, horizontalInput.y);
        this.sensitivity = sensitivity;
        this.gravity = gravity;
        this.overTopSpeedGravity = overTopSpeedGravity;
    }

    public float GetHorizSpeedThisFrame()
    {
        if (speedLastFrame > conventionalMaxSpeed)
        {
            return
                InputUtils.SmoothedInput(
                    speedLastFrame,
                    horizontalInput.magnitude * conventionalMaxSpeed,
                    sensitivity,
                    overTopSpeedGravity);
        }
        else
        {
            return
                InputUtils.SmoothedInput(
                    speedLastFrame,
                    horizontalInput.magnitude * conventionalMaxSpeed,
                    sensitivity,
                    gravity);
        }
    }
}
