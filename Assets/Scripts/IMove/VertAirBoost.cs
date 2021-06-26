using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertAirBoost : AMove
{
    float vertVel;
    MovementInputInfo mii;
    MovementInfo mi;

    public VertAirBoost(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, float propCharged) : base(mm)
    {
        vertVel = movementSettings.VertBoostMinVel + (propCharged * (movementSettings.VertBoostMaxVel - movementSettings.VertBoostMinVel));
        this.mii = mii;
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return
            InputUtils.SmoothedInput(
                mi.currentSpeed,
                mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                movementSettings.AirSensitivityX,
                movementSettings.AirGravityX);
    }

    public override float GetVertSpeedThisFrame()
    {
        vertVel -= movementSettings.VertBoostGravity * Time.deltaTime;
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

        return this;
    }

    public override string asString()
    {
        return "vertairboost";
    }
}
