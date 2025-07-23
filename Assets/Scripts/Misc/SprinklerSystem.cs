using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinklerSystem : MonoBehaviour
{
    [SerializeField] float maxDistance;
    [SerializeField] float fieldOfViewAngle;
    [SerializeField] bool constantlySprinkle;

    public void spraySprinkler()
    {
        List<GameObject> sprinklerReceiverObjects = new List<GameObject>();
        SprinklerReceive[] sprinklerReceivers = FindObjectsOfType<SprinklerReceive>();

        foreach (SprinklerReceive receiver in sprinklerReceivers)
        {
            if (IsObjectInFront(receiver.gameObject))
                receiver.receive();
        }
    }

    private void Update()
    {
        if (constantlySprinkle)
            spraySprinkler();
    }

    bool IsObjectInFront(GameObject obj)
    {
        float distance = Vector3.Distance(transform.position, obj.transform.position);
        if (distance > maxDistance)
        {
            return false;
        }

        Vector3 directionToObj = obj.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToObj);
        if (angle <= fieldOfViewAngle / 2f)
        {
            return true;
        }

        return false;
    }
}
