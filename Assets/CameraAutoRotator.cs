using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraUtils))]
public class CameraAutoRotator : MonoBehaviour
{
    [SerializeField] private float minInput;
    [SerializeField] private float autoSensitivity;
    [SerializeField] private float autoGravity;
    [SerializeField] private float maxAutoSpeed;
    CameraUtils camUtils;
    float horizAutoRotSpeed;

    private void Awake()
    {
        camUtils = GetComponent<CameraUtils>();
    }

    private void LateUpdate()
    {
        
    }
}
