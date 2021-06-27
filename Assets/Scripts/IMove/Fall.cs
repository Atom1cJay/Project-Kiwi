using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : AMove
{
    float vertVel;
    MovementInputInfo mii;
    MovementInfo mi;

    public Fall(MovementMaster mm, MovementInputInfo mii, MovementInfo mi) : base(mm)
    {
        vertVel = 0;
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
        vertVel -= movementSettings.DefaultGravity * Time.deltaTime;
        return vertVel;
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
        return "fall";
    }
}
