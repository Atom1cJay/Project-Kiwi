using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound Profile/Random Choice")]
public class RandomChoiceSoundProfile : SoundProfile
{
    [SerializeField] SoundProfile[] soundProfiles;
    int choice;

    public override void Initiate()
    {
        choice = Random.Range(0, soundProfiles.Length);
        soundProfiles[choice].Initiate();
    }

    public override void AdvanceTime()
    {
        soundProfiles[choice].AdvanceTime();
    }

    public override void Finish()
    {
        soundProfiles[choice].Finish();
    }
}
