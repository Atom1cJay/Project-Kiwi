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
    [SerializeField] float volume;
    private float volumeMultiplier = 1;

    [Range(-3f, 3f)]
    [SerializeField] float pitch;
    [SerializeField] bool sfx, music, loop;

    public string GetName()
    {
        return name;
    }
    public void Play(GameObject g)
    {
        g.AddComponent<AudioSource>();
        AudioSource audioSource = g.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;

        SoundPlayer sp = new SoundPlayer();
        audioSource.Play();

    }
}
