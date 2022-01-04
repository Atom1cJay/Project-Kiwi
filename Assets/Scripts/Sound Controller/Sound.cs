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
    public void Play(GameObject g)
    {
        AudioSource audioSource = g.AddComponent<AudioSource>();

        audioSource.clip = clip;
        pitch *= Random.Range(lowPitchRange, highPitchRange);

        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;

        SoundPlayer sp = g.AddComponent<SoundPlayer>();

        sp.SetUpPlayer(audioSource, SFX, name);

        audioSource.Play();

        AudioMasterController.instance.addToList(sp);

    }
}
