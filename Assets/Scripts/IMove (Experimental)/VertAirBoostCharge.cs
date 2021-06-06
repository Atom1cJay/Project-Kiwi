using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertAirBoostCharge : AMove
{
    public VertAirBoostCharge(HorizontalMovement hm) : base(hm) { }

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
            hm.currentSpeed, 0, 0, hm.airBoostChargeGravity);
    }
}
