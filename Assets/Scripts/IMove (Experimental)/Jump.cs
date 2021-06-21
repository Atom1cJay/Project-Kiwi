using System;
using UnityEngine;
using System.Collections;

public class Jump : AMove
{
    float gravity;
    float vertVel;
    MovementSettings ms;
    MovementInputInfo mii;
    MovementInfo mi;

    public Jump(MovementMaster mm, MovementSettings ms, MovementInputInfo mii, MovementInfo mi) : base(mm)
    {
        gravity = ms.jumpInitGravity;
        vertVel = ms.jumpInitVel;
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
        else if (mi.currentSpeed > ms.defaultMaxSpeedX)
        {
            toReturn =
                InputUtils.SmoothedInput(
                    mi.currentSpeed,
                    mii.GetHorizontalInput().magnitude * ms.defaultMaxSpeedX,
                    ms.airSensitivityX,
                    ms.airGravityXOverTopSpeed);
        }
        else
        {
            toReturn =
                InputUtils.SmoothedInput(
                    mi.currentSpeed,
                    mii.GetHorizontalInput().magnitude * ms.defaultMaxSpeedX,
                    ms.airSensitivityX,
                    ms.airGravityX);
        }

        return toReturn;
    }

    public override float GetVertSpeedThisFrame()
    {
        // Decide Gravity
        if (mm.JumpInputCancelled())
            gravity += ms.jumpCancelledGravityIncreaseRate * Time.deltaTime;
        else
            gravity += ms.jumpUncancelledGravityIncreaseRate * Time.deltaTime;

        if (gravity > ms.jumpMaxUncancelledGravity && !mm.JumpInputCancelled())
            gravity = ms.jumpMaxUncancelledGravity;
        else if (gravity > ms.jumpMaxCancelledGravity && mm.JumpInputCancelled())
            gravity = ms.jumpMaxCancelledGravity;

        // Effect Gravity
        vertVel -= gravity * Time.deltaTime;
        return vertVel;
    }

    private void OnJumpCanceled()
    {
        // todo add a point of no return?
        if (vertVel > 0)
        {
            vertVel *= ms.jumpVelMultiplierAtCancel;
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
        return "jump";
    }
}
   
