using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A sound profile which plays all of its "smaller" sound profiles at once.
/// </summary>
[CreateAssetMenu(menuName = "Sound Profile/Combined")]
public class CombinedSoundProfile : SoundProfile
{
    [SerializeField] SoundProfile[] soundProfiles;

    public override void Initiate()
    {
        foreach (SoundProfile profile in soundProfiles)
        {
            profile.Initiate();
        }
    }

    public override void AdvanceTime()
    {
        foreach (SoundProfile profile in soundProfiles)
        {
            profile.AdvanceTime();
        }
    }

    public override void Finish()
    {
        foreach (SoundProfile profile in soundProfiles)
        {
            profile.Finish();
        }
    }
}
