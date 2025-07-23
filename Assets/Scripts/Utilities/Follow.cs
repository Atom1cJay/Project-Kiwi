using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A more granular alternative to childing a gameobject to another one
public class Follow : MonoBehaviour
{
    [SerializeField] GameObject toFollow;
    [SerializeField] bool followRotationX;
    [SerializeField] bool followRotationY;
    [SerializeField] bool followRotationZ;
    Vector3 offset;
    Quaternion savedRot;

    void Awake()
    {
        offset = transform.position - toFollow.transform.position;
        savedRot = toFollow.transform.rotation;
    }

    void Update()
    {
        transform.position = toFollow.transform.position + offset;
        Quaternion rotationDiff = toFollow.transform.rotation * Quaternion.Inverse(savedRot);
        Vector3 eulerAnglesDiff = rotationDiff.eulerAngles;
        if (followRotationX)
        {
            transform.RotateAround(toFollow.transform.position, Vector3.right, eulerAnglesDiff.x);
        }
        if (followRotationY)
        {
            transform.RotateAround(toFollow.transform.position, Vector3.up, eulerAnglesDiff.y);
        }
        if (followRotationZ)
        {
            transform.RotateAround(toFollow.transform.position, Vector3.forward, eulerAnglesDiff.z);
        }
        savedRot = toFollow.transform.rotation;
    }
}
