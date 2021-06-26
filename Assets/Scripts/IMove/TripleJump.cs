using System;
using UnityEngine;
using System.Collections;

public class TripleJump : AMove
{
    float gravity;
    float vertVel;
    MovementSettings ms;
    MovementInputInfo mii;
    MovementInfo mi;

    public TripleJump(MovementMaster mm, MovementSettings ms, MovementInputInfo mii, MovementInfo mi) : base(mm)
    {
        gravity = ms.tjInitGravity;
        vertVel = ms.tjInitJumpVel;
        mm.mm_OnJumpCanceled.AddListener(OnJumpCanceled);
        this.ms = ms;
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        float toReturn;

        if (mm.IsAirReversing())
        {
            toReturn =
                InputUtils.SmoothedInput(
                    mi.currentSpeed,
                    -mii.GetHorizontalInput().magnitude * ms.defaultMaxSpeedX,
                    ms.airReverseSensitivityX,
                    ms.airReverseGravityX);
            if (toReturn < 0) toReturn = 0;
        }
        else
        {
            toReturn =
                InputUtils.SmoothedInput(
                    mi.currentSpeed,
                    mii.GetHorizontalInput().magnitude * ms.defaultMaxSpeedX,
                    ms.tjAirSensitivityX,
                    ms.tjInputGravityX);
        }

        return toReturn;
    }

    public override float GetVertSpeedThisFrame()
    {
        // Decide Gravity
        if (mm.JumpInputCancelled())
            gravity += ms.tjGravityIncRateAtCancel * Time.deltaTime;
        else
            gravity += ms.tjGravityIncRate * Time.deltaTime;

        if (gravity > ms.tjUncancelledMaxGravity && !mm.JumpInputCancelled())
            gravity = ms.tjUncancelledMaxGravity;
        else if (gravity > ms.tjCancelledMaxGravity && mm.JumpInputCancelled())
            gravity = ms.tjCancelledMaxGravity;

        // Effect Gravity
        vertVel -= gravity * Time.deltaTime;
        return vertVel;
    }

    /// <summary>
    /// Marks a jump as cancelled (meaning the arc should begin to decrease)
    /// if we are in the proper part of the jump.
    /// </summary>
    private void OnJumpCanceled()
    {
        // TODO point of no return?
        if (vertVel > 0)
        {
            vertVel *= ms.tjVelocityMultiplier;
        }
    }

    public override IMove GetNextMove()
    {
        if (mm.IsOnGround())
        {
            return new Run(mm, ms, mii, mi);
        }
        if (mm.IsAirDiving())
        {
            return new Dive(mm, ms, mii, mi);
        }
        if (mm.InAirBoostCharge())
        {
            return new HorizAirBoostCharge(mm, ms, mii, mi, vertVel);
        }
        if (mm.InVertAirBoostCharge())
        {
            return new VertAirBoostCharge(mm, ms, mii, mi, vertVel);
        }

        return this;
    }

    public override string asString()
    {
        return "triplejump";
    }
}
