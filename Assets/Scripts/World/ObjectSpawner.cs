using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] float preSpawnWait;
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
        yield return new WaitForSeconds(preSpawnWait);
        spawnsLeft = (numSpawns == 0) ? int.MaxValue : numSpawns;
        while (spawnsLeft > 0)
        {
            yield return new WaitForSeconds(interval);
            GameObject g = Instantiate(obj, transform);
            g.SetActive(true);
            OnSpawn(g);
            spawnsLeft -= 1;
        }
    }

    protected virtual void OnSpawn(GameObject go)
    {
        // Can be overridden
    }
}
