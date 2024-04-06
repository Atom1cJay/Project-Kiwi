using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovingPlatform : AMovingPlatform
{
    [SerializeField] GameObject waypointsParent;
    [SerializeField] int[] waypointsToGoToSlowly;
    [SerializeField] int startingIndex;
    [SerializeField] float movementSpeed, slowedSpeed, rotationSpeed, distanceToNextCheckpoint;

    [SerializeField] Vector3 pendulumRotatorExtreme;
    [SerializeField] Vector3 pendulumSpeed;

    Transform[] waypoints;
    Vector3 currentPendulum;
    int currentTargetIndex = 0;

    float currentSpeed = 1f;

    bool goSlowly = false;

    public override void Rotate()
    {
        transform.localEulerAngles -= currentPendulum;

        Vector3 targetDirection = waypoints[currentTargetIndex].position - transform.position;

        float singleStep = rotationSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, Mathf.Infinity);

        transform.rotation = Quaternion.LookRotation(newDirection);

        Vector3 cosValues = new Vector3(Mathf.Cos(Time.time * pendulumSpeed.x), Mathf.Cos(Time.time * pendulumSpeed.y), Mathf.Cos(Time.time * pendulumSpeed.z));
        currentPendulum = new Vector3(cosValues.x * pendulumRotatorExtreme.x, cosValues.y * pendulumRotatorExtreme.y, cosValues.z * pendulumRotatorExtreme.z);
        transform.localEulerAngles += currentPendulum;

    }

    public override void Translate()
    {
        if (Vector3.Distance(transform.position, waypoints[currentTargetIndex].position) <= distanceToNextCheckpoint)
        {
            NextTarget();
        }
        else
        {
            Vector3 targetVector = waypoints[currentTargetIndex].position - transform.position;

            currentSpeed = Mathf.Lerp(currentSpeed, (goSlowly ? slowedSpeed : movementSpeed), Time.deltaTime * 2);
            transform.position += targetVector.normalized * Time.deltaTime * currentSpeed;
        }
    }

    private void Update()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        if (waypointsParent != null)
        {
            waypoints = new Transform[waypointsParent.transform.childCount];

            for(int i = 0; i < waypointsParent.transform.childCount; i++)
            {
                waypoints[i] = waypointsParent.transform.GetChild(i);
            }
        }
        currentTargetIndex = startingIndex;

        if (currentTargetIndex >= waypoints.Length || currentTargetIndex < 0)
        {
            currentTargetIndex = 0;
        }

        goSlowly = false;

        foreach (int num in waypointsToGoToSlowly)
        {
            if (currentTargetIndex == num)
            {
                goSlowly = true;
                return;
            }
        }

        currentSpeed = (goSlowly ? slowedSpeed : movementSpeed);

        transform.position = waypoints[currentTargetIndex].position;

        Vector3 targetDirection = waypoints[currentTargetIndex].position - transform.position;

        float singleStep = rotationSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Mathf.Infinity, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }


    void NextTarget()
    {
        currentTargetIndex++;
        if (currentTargetIndex >= waypoints.Length)
        {
            currentTargetIndex = 0;
        }

        goSlowly = false;

        foreach (int num in waypointsToGoToSlowly)
        {
            if (currentTargetIndex == num)
            {
                goSlowly = true;
                return;
            }
        }
    }
}
