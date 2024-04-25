using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMusicControlSystem : MonoBehaviour
{
    [SerializeField] float transitionTime;
    public void startSong(string start)
    {
        AudioMasterController.instance.FadeInSound(start, transitionTime, gameObject);
    }
    public void fadeToNextSong(string from, string to)
    {
        Debug.Log("Fade to next song from| " + from + " | to " + to);
        AudioMasterController.instance.FadeInSound(to, transitionTime, gameObject);
        AudioMasterController.instance.FadeOutSound(from, transitionTime);
    }

}
