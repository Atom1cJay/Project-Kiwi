using System.Collections;
using PathCreation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathFollower : AMovingPlatform
{
    [SerializeField] PathCreator pathCreator;
    [SerializeField] protected float speed;
    [Header("Follow on Axis")]
    [SerializeField] bool x = true;
    [SerializeField] bool y = true;
    [SerializeField] bool z = true;
    [SerializeField] protected bool rotateWithPath = false;
    [SerializeField] bool stopAtEndOfPath = false;
    [SerializeField] bool destroyAtEndOfPath = false;
    [SerializeField] UnityEvent onReachEndOfPath;
    float distance;
    bool stopped;

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
        if (!stopped)
        {
            distance += speed * Time.deltaTime;
        }
        if (distance > pathCreator.path.length) // End of path
        {
            onReachEndOfPath.Invoke();
            if (destroyAtEndOfPath)
            {
                distance = pathCreator.path.length - 0.1f; // Just before end
                Destroy(gameObject);
            }
            else if (stopAtEndOfPath)
            {
                distance = pathCreator.path.length - 0.1f; // Just before end
                stopped = true;
            }
            else
            {
                distance -= pathCreator.path.length; // Loop
            }
        }
        Vector3 pathPos = pathCreator.path.GetPointAtDistance(distance);
        Vector3 pathDir = pathCreator.path.GetDirectionAtDistance(distance);
        Quaternion pathRot = Quaternion.LookRotation(pathDir);
        Vector3 desiredPos = transform.position = new Vector3(
            x ? pathPos.x : transform.position.x,
            y ? pathPos.y : transform.position.y,
            z ? pathPos.z : transform.position.z);
        transform.Translate(desiredPos - transform.position);
        if (rotateWithPath)
        {
            transform.rotation = pathRot;
        }
    }

    public void SetPathCreator(PathCreator pc)
    {
        this.pathCreator = pc;
        distance = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        Translate();
    }
}
