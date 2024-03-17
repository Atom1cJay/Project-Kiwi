using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField] GameObject toSpawn;

    void OnDestroy()
    {
        GameObject go = Instantiate(toSpawn);
        go.transform.position = transform.position;
    }
}
