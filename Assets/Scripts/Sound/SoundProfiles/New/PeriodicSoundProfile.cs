using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays sounds consistently depending on the passing of intervals.
/// </summary>
public class PeriodicSoundProfile : IntervalSoundProfile
{
    public override void AdvanceTime()
    {
        ContributeToInterval(Time.deltaTime);
    }

    public override void Finish() { }
}
