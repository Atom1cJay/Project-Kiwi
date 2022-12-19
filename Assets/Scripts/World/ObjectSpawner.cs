using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] float interval;
    [Header("0: No Limit")]
    [SerializeField] int numSpawns;
    int spawnsLeft;

    private void Start()
    {
        StartCoroutine("SpawnCoroutine");
    }

    IEnumerator SpawnCoroutine()
    {
        spawnsLeft = (numSpawns == 0) ? int.MaxValue : numSpawns;
        while (spawnsLeft > 0)
        {
            yield return new WaitForSeconds(interval);
            GameObject g = Instantiate(obj, transform);
            g.SetActive(true);
            spawnsLeft -= 1;
        }
    }
}
