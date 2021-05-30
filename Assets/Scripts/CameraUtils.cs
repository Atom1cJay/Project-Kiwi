using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtils : MonoBehaviour
{
    [SerializeField] private Transform target; // Object to look at / surround
    [SerializeField] private float radiusToTarget; // Distance from target
    [SerializeField] private float initHorizAngle = 0; // 0 = directly behind, 1.57 = to right. Initial value serialized.
    [SerializeField] private float initVertAngle = 0; // 0 = exactly aligned, 1.57 = on top. Initial value serialized.
    [SerializeField] private float vertAngleMin = 0f;
    [SerializeField] private float vertAngleMax = 1.57f;
    private float horizAngle;
    private float vertAngle;

    private void Awake()
    {
        horizAngle = initHorizAngle;
        vertAngle = initVertAngle;
    }

    /// <summary>
    /// Rotates the camera by the given amounts.
    /// </summary>
    /// <param name="horizAmount">The amount of horizontal rotation (degrees)</param>
    /// <param name="vertAmount">The amount of vertical rotation (degrees)</param>
    public void RotateBy(float horizAmount, float vertAmount)
    {
        horizAngle += horizAmount;
        vertAngle += vertAmount;
        vertAngle = Mathf.Clamp(vertAngle, vertAngleMin, vertAngleMax);
    }

    private void Update()
    {
        MoveCamera();
    }

    /// <summary>
    /// Based on the current horizontal / vertical angle, as well as the position/radius
    /// of the target, positions and angles the camera appropriately
    /// </summary>xw
    private void MoveCamera()
    {
        // Take horizontal angle into account
        float relativeX = radiusToTarget * Mathf.Sin(horizAngle);
        float relativeZ = radiusToTarget * -Mathf.Cos(horizAngle);
        // Take vertical angle into account
        relativeX *= Mathf.Cos(vertAngle);
        relativeZ *= Mathf.Cos(vertAngle);
        float relativeY = radiusToTarget * Mathf.Sin(vertAngle);
        // Change position
        transform.position = target.position + new Vector3(relativeX, relativeY, relativeZ);
        transform.LookAt(target);
    }
}
