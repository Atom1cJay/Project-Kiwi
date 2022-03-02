using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] float interval;

    private void Start()
    {
        InvokeRepeating("Spawn", 0, interval);
    }

    void Spawn()
    {
        Instantiate(obj, transform);
    }
}
