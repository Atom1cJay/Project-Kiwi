using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolatedRotator : MonoBehaviour
{
    [SerializeField] Vector3 rotation;


    void Update()
    {
        Vector3 currentRotation = transform.eulerAngles;
        Vector3 newRotation = new Vector3((currentRotation.x + rotation.x * Time.deltaTime) % 360f, (currentRotation.y + rotation.y * Time.deltaTime) % 360f, (currentRotation.z + rotation.z * Time.deltaTime) % 360);
        transform.eulerAngles = newRotation;
    }
}
