using System.Collections;
using PathCreation;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : AMovingPlatform
{
    [SerializeField] PathCreator pathCreator;
    [SerializeField] protected float speed;
    [Header("Follow on Axis")]
    [SerializeField] bool x = true;
    [SerializeField] bool y = true;
    [SerializeField] bool z = true;
    float distance;

    void Start()
    {
        distance = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    public override void Rotate()
    {
        // No rotation right now
    }

    public override void Translate()
    {
        distance += speed * Time.deltaTime;
        Vector3 pathPos = pathCreator.path.GetPointAtDistance(distance);
        Vector3 desiredPos = transform.position = new Vector3(
            x ? pathPos.x : transform.position.x,
            y ? pathPos.y : transform.position.y,
            z ? pathPos.z : transform.position.z);
        transform.Translate(desiredPos - transform.position);
    }
}
