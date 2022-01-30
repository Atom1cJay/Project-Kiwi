using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControlPoint : MonoBehaviour
{
    [SerializeField] public string song;
    [HideInInspector]
    public Vector3 position;

    void Awake()
    {
        position = transform.position;
    }

}
