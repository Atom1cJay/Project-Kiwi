using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickySoundPlayer : MonoBehaviour
{
    [SerializeField] Sound sound;

    void Start()
    {
        sound.Play(transform, transform);
    }
}
