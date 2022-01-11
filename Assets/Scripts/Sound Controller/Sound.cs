using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Sound", order = 1)]
//Represents a sound
public class Sound : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] AudioClip clip;

    [Range(0.0f, 1.0f)]
    [SerializeField] float volume = 1f;
    private float volumeMultiplier = 1;

    [Range(-3f, 3f)]
    [SerializeField] float pitch = 1;
    [SerializeField] bool SFX, MUSIC, loop, randomizePitch;

    [Range(.1f, 1.9f)]
    [SerializeField] float lowPitchRange, highPitchRange;

    public string GetName()
    {
        return name;
    }
    
    public AudioClip GetClip()
    {
        return clip;
    }

    public float GetVolume()
    {
        return volume;
    }

    public float GetPitch ()
    {
        return pitch;
    }

    public bool GetSFX()
    {
        return SFX;
    }

    public bool GetMUSIC()
    {
        return MUSIC;
    }

    public bool GetLoop()
    {
        return loop;
    }

    public bool GetRandomizePitch()
    {
        return randomizePitch;
    }

    public float GetLowPitchRange()
    {
        return lowPitchRange;
    }

    public float GetHighPitchRange()
    {
        return highPitchRange;
    }
}
