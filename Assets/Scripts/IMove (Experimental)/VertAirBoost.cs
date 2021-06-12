using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertAirBoost : AMove
{
    float vertVel;
    MovementSettings ms;
    MovementInputInfo mii;
    MovementInfo mi;

    public VertAirBoost(MovementMaster mm, MovementSettings ms, MovementInputInfo mii, MovementInfo mi, float propCharged) : base(mm)
    {
        vertVel = ms.vertBoostMinVel + (propCharged * (ms.vertBoostMaxVel - ms.vertBoostMinVel));
        this.ms = ms;
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return
            InputUtils.SmoothedInput(
                mi.currentSpeed,
                mii.GetHorizontalInput().magnitude * ms.defaultMaxSpeedX,
                ms.airSensitivityX,
                ms.airGravityX);
    }

    public override float GetVertSpeedThisFrame()
    {
        vertVel -= ms.vertBoostGravity * Time.deltaTime;
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

        return this;
    }
}
