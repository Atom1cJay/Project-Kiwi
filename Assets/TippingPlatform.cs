using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class TippingPlatform : MonoBehaviour
{
    //private float rotX;
    //private float rotZ;
    private float angle;
    [SerializeField] Renderer dimensionsRenderer;
    [SerializeField] Transform toRotate;
    [SerializeField] float rotAcceleration;

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
            StartCoroutine("Fall");
            //rotX = ratioX * 2 * rotMax;
            //rotZ = ratioZ * 2 * rotMax;
        }
    }

    IEnumerator Fall()
    {
        float xRot = 0;
        float zRot = 0;
        float xRotMultiplier = Mathf.Cos(angle);
        float zRotMultiplier = Mathf.Sin(angle);
    
        while (true)
        {
            xRot += rotAcceleration * xRotMultiplier * Time.fixedDeltaTime;
            zRot += rotAcceleration * zRotMultiplier * Time.fixedDeltaTime;
            toRotate.Rotate(new Vector3(zRot, 0, -xRot) * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
