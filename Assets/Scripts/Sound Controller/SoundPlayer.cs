using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    bool started = false;
    bool sfx;
    bool music;
    AudioSource player;

    private void Update()
    {
        if (started)
        {
            if(!player.isPlaying)
            {
                Destroy(player);
                Destroy(this);
            }
        }
        
    }

    public void SetUpPlayer(AudioSource audioSource, bool sfx, bool music)
    {
        this.sfx = sfx;
        this.music = music;
        player = audioSource;
        player.Play();
        started = true;
    }
}
