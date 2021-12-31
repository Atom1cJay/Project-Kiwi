using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] List<Sound> soundList;
    [SerializeField] GameObject playerSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlaySound(string name)
    {
        foreach (Sound s in soundList)
        {
            if (s.GetName().Equals(name))
            {
                s.Play(playerSound);
            }
        }
    }

    public void PlaySound(string name, GameObject g)
    {
        foreach (Sound s in soundList)
        {
            if (s.GetName().Equals(name))
            {
                s.Play(g);
            }
        }
    }
}
