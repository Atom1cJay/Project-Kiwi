using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] string song1, song2;
    [SerializeField] GameObject location1,player;
    [SerializeField] float dist;

    bool updating = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(finishUpdatingd());
    }

    // Update is called once per frame
    void Update()
    {
        if (!updating)
        {
            if (Vector3.Distance(player.transform.position, location1.transform.position) < dist)
            {
                //song 1

                if (!AudioMasterController.instance.isPlaying(song1))
                {
                    AudioMasterController.instance.FadeInSound(song1, 3f, gameObject);
                    AudioMasterController.instance.FadeOutSound(song2, 3f);
                    updating = true;
                    StartCoroutine(finishUpdating(3f));
                }
            }
            else
            {
                //song 2

                if (!AudioMasterController.instance.isPlaying(song2))
                {
                    AudioMasterController.instance.FadeInSound(song2, 3f, gameObject);
                    AudioMasterController.instance.FadeOutSound(song1, 3f);
                    updating = true;
                    StartCoroutine(finishUpdating(3f));
                }
            }
        }
    }


    IEnumerator finishUpdating(float time)
    {
        yield return new WaitForSeconds(time);
        updating = false;
    }
    IEnumerator finishUpdatingd()
    {
        yield return new WaitForSeconds(5f);
        AudioMasterController.instance.FadeInSound(song1, 3f, gameObject);
        updating = false;
    }
    
}
