using UnityEngine;
using System.Collections;

public class HorizAirBoost : AMove
{
    public HorizAirBoost(HorizontalMovement hm) : base(hm) { }

    public override float GetHorizSpeedThisFrame()
    {
        return hm.airBoostSpeed;
    }
}
