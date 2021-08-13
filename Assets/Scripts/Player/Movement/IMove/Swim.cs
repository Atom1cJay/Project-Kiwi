using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Represents movement in the water.
/// </summary>
public class Swim : AMove
{
    Vector2 horizVector;
    bool jumpPending;

    /// <summary>
    /// Constructs a Swim, initializing the objects that hold all the
    /// information it needs to function.
    /// </summary>
    /// <param name="mii">Information on the player's input</param>
    /// <param name="mi">Information on the state of the player</param>
    /// <param name="ms">Constants related to movement</param>
    /// <param name="horizVector">The horizontal velocity going into this move</param>
    public Swim(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(ms, mi, mii)
    {
        this.horizVector = horizVector;
        mii.OnJump.AddListener(() => jumpPending = true);
    }

    public override void AdvanceTime()
    {
        horizVector += mii.GetRelativeHorizontalInputToCamera() * movementSettings.SwimSensitivityNormal * Time.deltaTime;
        if (horizVector.magnitude > movementSettings.SwimMaxSpeedNormal)
        {
            horizVector = horizVector.normalized * movementSettings.SwimMaxSpeedNormal;
        }
        if (mii.GetRelativeHorizontalInputToCamera().magnitude < 0.1f)
        {
            float magn = horizVector.magnitude;
            magn -= movementSettings.SwimGravity * Time.deltaTime;
            horizVector = horizVector.normalized * magn;
        }
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return horizVector;
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override float GetRotationSpeed()
    {
        return movementSettings.SwimRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (jumpPending)
        {
            return new Jump(mii, mi, movementSettings, horizVector.magnitude);
        }
        if (mi.TouchingGround())
        {
            if (horizVector.magnitude > 0)
            {
                return new Run(mii, mi, movementSettings, horizVector);
            }
            else
            {
                return new Idle(mii, mi, movementSettings);
            }
        }
        return this;
    }

    public override string AsString()
    {
        return "swim";
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
}