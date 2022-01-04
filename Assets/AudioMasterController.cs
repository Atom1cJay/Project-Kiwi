using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMasterController : MonoBehaviour
{
    [HideInInspector]
    public static AudioMasterController instance;

    [SerializeField] List<Sound> soundList;
    [SerializeField] float fadeOutTime;
    List<SoundPlayer> allPlayers;
    GameObject playerSound;
    float sfxMult, musicMult;

    // Start is called before the first frame update
    void Start()
    {
        sfxMult = 1f;
        musicMult = 1f;
        allPlayers = new List<SoundPlayer>();

        if (instance == null)
        {
            instance = this;
        }

        playerSound = gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StopSound(string name)
    {
        foreach (SoundPlayer sp in allPlayers)
        {
            if (sp.getName().Equals(name))
            {
                allPlayers.Remove(sp);
                sp.fadeOut(.5f);
            }
        }
    }

    public bool isPlaying(string name)
    {
        foreach (SoundPlayer sp in allPlayers)
        {
            if (sp.getName().Equals(name))
            {
                return true;
            }
        }

        return false;
    }
 
    public void addToList (SoundPlayer sp)
    {
        allPlayers.Add(sp);
    }

    public float getSFXMult()
    {
        return sfxMult;
    }

    public float getMusicMult()
    {
        return musicMult;
    }

    public void PlaySound(string name)
    {
        PlaySound(name, playerSound);
    }

    public void PlaySound(string name, GameObject g)
    {

        //new audio player
        GameObject newObject = new GameObject();
        newObject.name = name + " sound!";
        newObject.transform.parent = g.transform;

        bool playedSong = false;

        foreach (Sound s in soundList)
        {
            if (s.GetName().Equals(name))
            {
                s.Play(newObject);
                playedSong = true;
            }
        }

        if (!playedSong)
        {
            Destroy(newObject);
            Debug.Log("Sound: " + name + " not found!");
        }

    }

    public float getFadeOutTime()
    {
        return fadeOutTime;
    }

}
