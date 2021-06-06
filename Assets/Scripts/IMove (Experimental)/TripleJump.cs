using UnityEngine;
using System.Collections;

public class TripleJump : AMove
{
    public TripleJump(HorizontalMovement hm) : base(hm) { }

    public override float GetHorizSpeedThisFrame()
    {
        float toReturn;

        if (hm.isAirReversing())
        {
            toReturn =
                InputUtils.SmoothedInput(
                    hm.currentSpeed,
                    -hm.getHorizontalInput().magnitude * hm.defaultMaxSpeed,
                    hm.airReverseSensitivity,
                    hm.airReverseGravity);
            if (toReturn < 0) toReturn = 0;
        }
        else
        {
            toReturn =
                InputUtils.SmoothedInput(
                    hm.currentSpeed,
                    hm.getHorizontalInput().magnitude * hm.defaultMaxSpeed,
                    hm.tjAirSensitivity,
                    hm.tjAirGravity);
        }

        return toReturn;
    }
}
