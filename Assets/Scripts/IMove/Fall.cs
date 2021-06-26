using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : AMove
{
    float vertVel;
    MovementSettings ms;
    MovementInputInfo mii;
    MovementInfo mi;

    public Fall(MovementMaster mm, MovementSettings ms, MovementInputInfo mii, MovementInfo mi) : base(mm)
    {
        vertVel = 0;
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
        vertVel -= ms.defaultGravity * Time.deltaTime;
        return vertVel;
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
        return "fall";
    }
}
