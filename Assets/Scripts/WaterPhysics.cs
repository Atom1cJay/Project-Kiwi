using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPhysics : MonoBehaviour { 

    private float angle;
Renderer dimensionsRenderer;
Transform toRotate;
[SerializeField] float rotAcceleration, maxAngle, returnSpeed;
float xRot;
float zRot;
float xRotMultiplier;
float zRotMultiplier;
bool rotating;

    Vector3 initialRotation;

    private void Awake()
    {
        dimensionsRenderer = GetComponent<Renderer>();
        toRotate = transform;
        initialRotation = toRotate.localEulerAngles;
    }


    // Happens when the player touches the top of the platform (the trigger
    // collider will have to be appropriately adjusted)
    private void OnCollisionEnter(Collision other)
{
    if (MovementInfo.instance.GetGroundDetector().CollidingWith() != gameObject)
    {
        return;
    }

    if (angle != 0)
    {
        return; // Can only get angles once
    }

    if (other.gameObject.layer == 9)
    {
        float sizeX = dimensionsRenderer.bounds.max.x - dimensionsRenderer.bounds.min.x;
        float sizeZ = dimensionsRenderer.bounds.max.z - dimensionsRenderer.bounds.min.z;
        Vector3 contactPoint = other.gameObject.transform.position;
        float ratioX = (contactPoint.x - transform.position.x) / sizeX;
        float ratioZ = (contactPoint.z - transform.position.z) / sizeZ;
        angle = Mathf.Atan2(ratioZ, ratioX);
        GetComponent<Collider>().isTrigger = false;
        xRot = 0;
        zRot = 0;
        xRotMultiplier = Mathf.Cos(angle);
        zRotMultiplier = Mathf.Sin(angle);

        xRot += rotAcceleration * xRotMultiplier;
        zRot += rotAcceleration * zRotMultiplier;
        rotating = true;
    }
}

private void FixedUpdate()
{


        if (rotating)
        {
            toRotate.Rotate(new Vector3(zRot, 0, -xRot) * Time.fixedDeltaTime);
        }

        xRot = Mathf.Lerp(xRot, 0f, Time.fixedDeltaTime * returnSpeed);
        zRot = Mathf.Lerp(zRot, 0f, Time.fixedDeltaTime * returnSpeed);

        toRotate.transform.localEulerAngles = Vector3.Lerp(toRotate.transform.localEulerAngles, initialRotation, Time.fixedDeltaTime * returnSpeed);

        if (Mathf.Approximately(xRot, 0f))
            xRot = 0f;

        if (Mathf.Approximately(zRot, 0f))
            zRot = 0f;

        if (xRot == 0f && zRot == 0f)
            rotating = false;

    }
}
