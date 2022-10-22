using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides general functionality for any sound profile where one or more sounds
/// can be played multiple times.
/// </summary>
public abstract class ContinuousSoundProfile : SoundProfile
{
    public enum SoundIterationType
    {
        Cycle,
        RandomRepeatsAllowed,
        RandomNoRepeatsAllowed
    }

    [SerializeField] Sound[] sounds;
    [SerializeField] SoundIterationType soundIterationType;
    int currentSound = 0;

    /// <summary>
    /// Choose which sound should play next.
    /// </summary>
    void SelectNextSound()
    {
        if (sounds.Length < 2)
        {
            return;
        }
        switch (soundIterationType)
        {
            case SoundIterationType.Cycle:
                currentSound = (currentSound + 1) % sounds.Length;
                break;
            case SoundIterationType.RandomRepeatsAllowed:
                currentSound = Random.Range(0, sounds.Length);
                break;
            case SoundIterationType.RandomNoRepeatsAllowed:
                int oldSound = currentSound;
                while (currentSound == oldSound)
                {
                    currentSound = Random.Range(0, sounds.Length);
                }
                break;
        }
    }

    /// <summary>
    /// Play the next sound.
    /// </summary>
    protected void PlayNextSound()
    {
        AudioMasterController.instance.PlaySound(sounds[currentSound].name);
        SelectNextSound();
    }
}
