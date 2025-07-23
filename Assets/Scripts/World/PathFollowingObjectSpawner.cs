using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathFollowingObjectSpawner : ObjectSpawner
{
    [SerializeField] PathCreator pathCreator;

    protected override void OnSpawn(GameObject go)
    {
        go.GetComponent<PathFollower>().SetPathCreator(pathCreator);
    }
}
