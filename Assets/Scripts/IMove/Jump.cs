﻿using System;
using UnityEngine;
using System.Collections;

public class Jump : AMove
{
    float gravity;
    float vertVel;
    MovementInputInfo mii;
    MovementInfo mi;

    public Jump(MovementMaster mm, MovementInputInfo mii, MovementInfo mi) : base(mm)
    {
        gravity = movementSettings.JumpInitGravity;
        vertVel = movementSettings.JumpInitVel;
        mm.mm_OnJumpCanceled.AddListener(OnJumpCanceled);
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
                    mi.currentSpeedHoriz,
                    -mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirReverseSensitivityX,
                    movementSettings.AirReverseGravityX);
            if (toReturn < 0) toReturn = 0;
        }
        else if (mi.currentSpeedHoriz > movementSettings.MaxSpeed)
        {
            toReturn =
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirSensitivityX,
                    movementSettings.AirGravityXOverTopSpeed);
        }
        else
        {
            toReturn =
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.AirSensitivityX,
                    movementSettings.AirGravityX);
        }

        return toReturn;
    }

    public override float GetVertSpeedThisFrame()
    {
        // Decide Gravity
        if (mm.JumpInputCancelled())
            gravity += movementSettings.JumpCancelledGravityIncrease * Time.deltaTime;
        else
            gravity += movementSettings.JumpUncancelledGravityIncrease * Time.deltaTime;

        if (gravity > movementSettings.JumpMaxUncancelledGravity && !mm.JumpInputCancelled())
            gravity = movementSettings.JumpMaxUncancelledGravity;
        else if (gravity > movementSettings.JumpMaxCancelledGravity && mm.JumpInputCancelled())
            gravity = movementSettings.JumpMaxCancelledGravity;

        // Effect Gravity
        vertVel -= gravity * Time.deltaTime;
        return vertVel;
    }

    private void OnJumpCanceled()
    {
        // todo add a point of no return?
        if (vertVel > 0)
        {
            vertVel *= movementSettings.JumpVelMultiplierAtCancel;
        }
    }

    public override IMove GetNextMove()
    {
        if (mm.IsOnGround())
        {
            return new Run(mm, mii, mi);
        }
        if (mm.IsAirDiving())
        {
            return new Dive(mm, mii, mi);
        }
        if (mm.InAirBoostCharge())
        {
            return new HorizAirBoostCharge(mm, mii, mi, vertVel);
        }
        if (mm.InVertAirBoostCharge())
        {
            return new VertAirBoostCharge(mm, mii, mi, vertVel);
        }

        return this;
    }

    public override string asString()
    {
        return "jump";
    }
}
   
