using UnityEngine;
using System.Collections;

public class Jump : AMove
{
    public Jump(HorizontalMovement hm) : base(hm) { }

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
        else if (hm.currentSpeed > hm.defaultMaxSpeed)
        {
            toReturn =
                InputUtils.SmoothedInput(
                    hm.currentSpeed,
                    hm.getHorizontalInput().magnitude * hm.defaultMaxSpeed,
                    hm.sensitivity,
                    hm.overTopSpeedAirGravity);
        }
        else
        {
            toReturn =
                InputUtils.SmoothedInput(
                    hm.currentSpeed,
                    hm.getHorizontalInput().magnitude * hm.defaultMaxSpeed,
                    hm.airSensitivity,
                    hm.airGravity);
        }

        return toReturn;
    }
}
