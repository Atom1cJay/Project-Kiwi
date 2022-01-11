using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    bool started = false;
    bool sfx;
    bool fadeOutStart = false;
    bool fadeIn = false;

    float goalVolume;
    float fadeOutTime = 1f;
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
                Destroy(gameObject);
            }
        }

        if (sfx)
        {
            player.pitch = player.pitch * AudioMasterController.instance.getSFXMult();
        }
        else
        {
            player.pitch = player.pitch * AudioMasterController.instance.getMusicMult();
        }

        if(fadeIn)
        {

            player.volume += fadeInTime * Time.deltaTime;
            if (player.volume >= goalVolume)
            {
                fadeIn = false;
            }
        }
        else if (fadeOutStart)
        {

            player.volume -= fadeOutTime * Time.deltaTime;
            if (player.volume <= .01f)
            {
                Destroy(gameObject);
            }
        }

    }

    public void fadeOut(float time)
    {
        fadeOutTime = player.volume / time;
        fadeOutStart = true;
    }

    public void SetUpPlayer(AudioSource audioSource, bool sfx, string name)
    {
        this.sfx = sfx;
        player = audioSource;
        soundName = name;
        player.Play();
        started = true;
    }

    public void SetUpFadeIn(AudioSource audioSource, bool sfx, string name, float time)
    {
        this.sfx = sfx;
        fadeIn = true;
        player = audioSource;
        goalVolume = player.volume;
        fadeInTime = goalVolume / time;
        player.volume = 0f;
        soundName = name;
        player.Play();
        started = true;
    }
}
