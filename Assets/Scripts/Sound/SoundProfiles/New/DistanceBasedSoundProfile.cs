using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound Profile/Distance Based")]
public class DistanceBasedSoundProfile : IntervalSoundProfile
{
    public override void AdvanceTime()
    {
        ContributeToInterval(MoveExecuter.GetCurrentMove().GetHorizSpeedThisFrame().magnitude * Time.deltaTime);
    }

    public override void Finish() { }
}
