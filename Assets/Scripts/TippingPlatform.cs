using System.Collections;
using UnityEngine;

/// <summary>
/// To be placed on the GameObject with the trigger collider which triggers
/// the motion.
/// </summary>
public class TippingPlatform : MonoBehaviour
{
    private float angle;
    [SerializeField] Renderer dimensionsRenderer;
    [SerializeField] Transform toRotate;
    [SerializeField] float rotAcceleration;
    float xRot;
    float zRot;
    float xRotMultiplier;
    float zRotMultiplier;
    bool rotating;

    // Happens when the player touches the top of the platform (the trigger
    // collider will have to be appropriately adjusted)
    private void OnTriggerEnter(Collider other)
    {
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
            rotating = true;
        }
    }

    private void FixedUpdate()
    {
        if (rotating)
        {
            xRot += rotAcceleration * xRotMultiplier * Time.fixedDeltaTime;
            zRot += rotAcceleration * zRotMultiplier * Time.fixedDeltaTime;
            toRotate.Rotate(new Vector3(zRot, 0, -xRot) * Time.fixedDeltaTime);
        }
    }
}
