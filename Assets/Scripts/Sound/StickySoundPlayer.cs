using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickySoundPlayer : MonoBehaviour
{
    [SerializeField] Sound sound;

    void Start()
    {
        if (AudioMasterController.instance != null)
            sound.Play(transform, transform);
    }
}
