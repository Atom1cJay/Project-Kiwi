using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMusicControlSystem : MonoBehaviour
{
    [SerializeField] float transitionTime;
    string currentSong;

    public void startSong(string start)
    {
        AudioMasterController.instance.FadeInSound(start, transitionTime, gameObject);
        currentSong = start;
    }

    public void autoFade(string nextSong)
    {
        Debug.Log("auto Fade to next song from| " + currentSong + " | to " + nextSong);
        AudioMasterController.instance.FadeInSound(nextSong, transitionTime, gameObject);
        AudioMasterController.instance.FadeOutSound(currentSong, transitionTime);
        currentSong = nextSong;
    }

    public void fadeToNextSong(string from, string to)
    {
        Debug.Log("Fade to next song from| " + from + " | to " + to);
        AudioMasterController.instance.FadeInSound(to, transitionTime, gameObject);
        AudioMasterController.instance.FadeOutSound(from, transitionTime);
        currentSong = to;
    }

}
