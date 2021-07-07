using System;
using UnityEngine;

public class Glidev2 : AMove
{
    bool groundPoundPending; // Temp
    bool glidePending; // Temp
    float vertVel;
    float horizVel;
    float tilt; // Angle forward/backward. Positive = forward

    public Glidev2(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel, float vertVel) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mii.OnGlide.AddListener(() => glidePending = true);
        tilt = 10 * Mathf.Deg2Rad;
    }

    public override void AdvanceTime()
    {
        Debug.Log(tilt * Mathf.Rad2Deg);
        tilt = InputUtils.SmoothedInput(tilt, mii.GetHorizontalInput().y * movementSettings.GlideMaxTilt, movementSettings.GlideTiltSensitivity, movementSettings.GlideTiltSensitivity);
        tilt = Mathf.Clamp(tilt, -movementSettings.GlideMaxTilt * Mathf.Deg2Rad, movementSettings.GlideMaxTilt * Mathf.Deg2Rad);
        horizVel += movementSettings.GlideWeight * Mathf.Sin(tilt) * Time.deltaTime;
        vertVel += horizVel * -Mathf.Tan(tilt) * Time.deltaTime;
        if (vertVel > 0)
        {
            vertVel -= movementSettings.GlideGravity * Time.deltaTime;
        }
        if (horizVel < 0)
        {
            horizVel = 0;
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
        return 0;
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
                return new Run(mii, mi, movementSettings, horizVel);
            }
        }
        else if (glidePending)
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

    public override string AsString()
    {
        return "glide";
    }
}
