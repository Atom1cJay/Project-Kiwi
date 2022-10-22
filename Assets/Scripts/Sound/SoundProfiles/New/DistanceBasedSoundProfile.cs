using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBasedSoundProfile : IntervalSoundProfile
{
    public override void AdvanceTime()
    {
        ContributeToInterval(Time.deltaTime);
    }

    public override void Finish() { }
}
