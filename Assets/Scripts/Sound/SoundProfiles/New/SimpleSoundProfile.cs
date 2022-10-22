using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSoundProfile : SoundProfile
{
    [SerializeField] Sound sound;
    [SerializeField] bool stopSoundAtEnd;

    public override void Initiate()
    {
        AudioMasterController.instance.PlaySound(sound.name);
    }

    public override void AdvanceTime() { }

    public override void Finish()
    {
        if (stopSoundAtEnd)
        {
            AudioMasterController.instance.StopSound(sound.name);
        }
    }
}
