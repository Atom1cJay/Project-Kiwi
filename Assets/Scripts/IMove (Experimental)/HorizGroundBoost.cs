using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizGroundBoost : AMove
{
    public HorizGroundBoost(HorizontalMovement hm) : base(hm) { }

    public override float GetHorizSpeedThisFrame()
    {
        return InputUtils.SmoothedInput(
            hm.currentSpeed,
            hm.groundBoostSpeed,
            hm.groundBoostSensitivity,
            hm.groundBoostGravity);
    }
}
