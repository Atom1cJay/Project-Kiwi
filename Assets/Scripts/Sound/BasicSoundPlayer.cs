using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSoundPlayer : MonoBehaviour
{
    [SerializeField] float durationToFadeIn;
    [SerializeField] bool playOnAwake;
    [SerializeField] Sound sound;

    public void playSound()
    {
        sound.FadeIn(durationToFadeIn, gameObject.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playOnAwake)
            playSound();
    }

}
