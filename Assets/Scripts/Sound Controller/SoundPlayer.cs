using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    bool started = false;
    bool sfx, music;
    bool fadeOutStart = false;
    bool fadeIn = false;


    float goalVolume;
    float fadeOutTime = 1f;
    float currentVolume = 0f;

    float fadeInTime = 1f;
    AudioSource player;
    string soundName;

    public string getName()
    {
        return soundName;
    }

    private void Update()
    {

        if (started)
        {
            if(!player.isPlaying)
            {
                AudioMasterController.instance.RemoveFromList(this);
                Destroy(gameObject);
            }
        }

        if(fadeIn)
        {

            currentVolume += fadeInTime * Time.deltaTime;
            if (currentVolume >= goalVolume)
            {
                fadeIn = false;
            }
        }
        else if (fadeOutStart)
        {

            currentVolume -= fadeOutTime * Time.deltaTime;
            if (currentVolume <= .01f)
            {

                Destroy(gameObject);
            }
        }

        UpdateMultipliers();

    }

    public void fadeOut(float time)
    {
        fadeOutTime = player.volume / time;
        fadeOutStart = true;
    }

    public void SetUpPlayer(AudioSource audioSource, bool sfx, bool music, string name)
    {
        this.sfx = sfx;
        this.music = music;

        player = audioSource;
        goalVolume = player.volume;
        currentVolume = player.volume;
        soundName = name;
        UpdateMultipliers();
        player.Play();
        started = true;
    }

    public void SetUpFadeIn(AudioSource audioSource, bool sfx, bool music, string name, float time)
    {
        this.sfx = sfx;
        this.music = music;

        fadeIn = true;
        player = audioSource;
        goalVolume = player.volume;
        fadeInTime = goalVolume / time;
        player.volume = 0f;
        soundName = name;
        player.Play();
        started = true;
    }

    public void UpdateMultipliers()
    {

        float sfxMult = AudioMasterController.instance.getSFXMult();
        float musicMult = AudioMasterController.instance.getMusicMult();


        if (sfx)
            player.volume = currentVolume * sfxMult;

        if (music)
            player.volume = currentVolume * musicMult;
    }
}
