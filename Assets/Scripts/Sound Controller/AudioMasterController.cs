using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMasterController : MonoBehaviour
{
    [HideInInspector]
    public static AudioMasterController instance;

    [SerializeField] List<Sound> soundList;
    List<SoundPlayer> allPlayers;
    GameObject playerSound;
    float sfxMult, musicMult;

    // Start is called before the first frame update
    void Awake()
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
        if (isPlaying(name))
        {
            FadeOutSound(name, 0.5f);
        }
    }

    public bool isPlaying(string name)
    {
        for (int i = 0; i < allPlayers.Count; i++)
        {
            SoundPlayer sp = allPlayers[i];
            if (sp.getName().Equals(name))
            {
                return true;
            }
        }

        return false;
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
               Play(newObject, s);
                playedSong = true;
            }
        }

        if (!playedSong)
        {
            Destroy(newObject);
            Debug.Log("Sound: " + name + " not found!");
        }

    }

    void Play(GameObject g, Sound s)
    {
        AudioSource audioSource = g.AddComponent<AudioSource>();

        audioSource.clip = s.GetClip();
        audioSource.pitch = s.GetPitch();

        if (s.GetRandomizePitch())
        {
            float tempPitch = s.GetPitch() * Random.Range(s.GetLowPitchRange(), s.GetHighPitchRange());
            audioSource.pitch = tempPitch;
        }

        audioSource.volume = s.GetVolume();
        audioSource.loop = s.GetLoop();

        SoundPlayer sp = g.AddComponent<SoundPlayer>();

        sp.SetUpPlayer(audioSource, s.GetSFX(), s.GetMUSIC(), s.GetName());

        allPlayers.Add(sp);

    }
    void FadeIn(Sound s, float time, GameObject g)
    {

        AudioSource audioSource = g.AddComponent<AudioSource>();

        audioSource.clip = s.GetClip();
        audioSource.pitch = s.GetPitch();

        if (s.GetRandomizePitch())
        {
            float tempPitch = s.GetPitch() * Random.Range(s.GetLowPitchRange(), s.GetHighPitchRange());
            audioSource.pitch = tempPitch;
        }

        audioSource.volume = s.GetVolume();
        audioSource.loop = s.GetLoop();

        SoundPlayer sp = g.AddComponent<SoundPlayer>();

        sp.SetUpFadeIn(audioSource, s.GetSFX(), s.GetMUSIC(), s.GetName(), time);

        allPlayers.Add(sp);

    }
    public void FadeInSound(string name, float time)
    {
        FadeInSound(name, time, playerSound);
    }

    public void FadeInSound(string name,float time, GameObject g)
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
                FadeIn(s, time, newObject);
                playedSong = true;
            }
        }

        if (!playedSong)
        {
            Destroy(newObject);
            Debug.Log("Sound: " + name + " not found!");
        }
    }

    public void FadeOutSound(string name, float time)
    {

        for (int i = 0; i < allPlayers.Count; i++)
        {
            SoundPlayer sp = allPlayers[i];
            if (sp.getName().Equals(name))
            {
                allPlayers.Remove(sp);
                sp.fadeOut(time);
            }
        }
    }

    public void RemoveFromList(SoundPlayer sp)
    {
        allPlayers.Remove(sp);
    }

    public void SetSFX(float mult)
    {
        sfxMult = mult;
    }
    public void SetMusic(float mult)
    {
        musicMult = mult;
    }

    public void UpdateMultipliers()
    {
        foreach (SoundPlayer sp in allPlayers)
        {
            sp.UpdateMultipliers();
        }
    }
}
