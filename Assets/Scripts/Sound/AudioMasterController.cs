using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMasterController : MonoBehaviour
{
    [HideInInspector]
    public static AudioMasterController instance;

    [SerializeField] List<Sound> soundList;
    List<SoundPlayer> allPlayers;
    float sfxMult, musicMult;

    void Awake()
    {
        sfxMult = 1f;
        musicMult = 1f;
        allPlayers = new List<SoundPlayer>();

        if (instance == null)
        {
            instance = this;
        }
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

    void PlaySoundOnGameObject(GameObject g, Sound s)
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

    private void PlaySound(Sound s, Transform parent)
    {
        GameObject soundGameObject = new GameObject();
        soundGameObject.name = s.GetName() + " sound!";
        soundGameObject.transform.parent = parent;
        PlaySoundOnGameObject(soundGameObject, s);
    }

    public void PlaySound(Sound s)
    {
        PlaySound(s, this.gameObject.transform);
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
        FadeInSound(name, time, this.gameObject);
    }

    public void FadeInSound(string name, float time, GameObject g)
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