using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtils : MonoBehaviour
{
    [SerializeField] private Transform target; // Object to look at / surround
    [SerializeField] private Transform player; // For rotation reference
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

    /// <summary>
    /// Takes the given amount of time to rotate to the given radian rotation.
    /// </summary>
    public void RotateToTargetY(float time)
    {
        StopCoroutine("RotateTowardsY");
        StartCoroutine("RotateTowardsY", time);
    }

    IEnumerator RotateTowardsY(float time)
    {
        float initialY = -transform.eulerAngles.y * Mathf.Deg2Rad;
        float goalY = -player.eulerAngles.y * Mathf.Deg2Rad;
        Quaternion angle1 = Quaternion.Euler(0, initialY * Mathf.Rad2Deg, 0);
        Quaternion angle2 = Quaternion.Euler(0, goalY * Mathf.Rad2Deg, 0);
        float rotationSpeed = Mathf.Abs(Quaternion.Angle(angle1, angle2)) / time;
        float elapsed = 0;
        while (elapsed < time)
        {
            Quaternion shit = Quaternion.Euler(0, -transform.eulerAngles.y, 0);
            Quaternion shit2 = Quaternion.Euler(0, -player.eulerAngles.y, 0);
            Quaternion newRot = Quaternion.RotateTowards(shit, shit2, rotationSpeed * Time.deltaTime);
            horizAngle = newRot.eulerAngles.y * Mathf.Deg2Rad;
            MoveCamera();
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void Update()
    {
        MoveCamera();
    }

    private void FixedUpdate()
    {
        MoveCamera();
    }

    private void LateUpdate()
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

    /// <summary>
    /// Moves the camera closer to the back of the player by the ratio given.
    /// </summary>
    public void RotateToBackBy(float ratio)
    {
        Quaternion angle1 = Quaternion.Euler(0, horizAngle * Mathf.Rad2Deg, 0);
        Quaternion angle2 = Quaternion.Euler(0, -player.eulerAngles.y, 0);
        Quaternion newRot = Quaternion.RotateTowards(angle1, angle2, Quaternion.Angle(angle1, angle2) * ratio);
        horizAngle = newRot.eulerAngles.y * Mathf.Deg2Rad;
    }

    /// <summary>
    /// Moves the camera closer to the given vertical angle (degrees) by the ratio given.
    /// </summary>
    public void RotateToVertAngle(float ratio, float angle)
    {
        Quaternion angle1 = Quaternion.Euler(vertAngle * Mathf.Rad2Deg, 0, 0);
        Quaternion angle2 = Quaternion.Euler(angle, 0, 0);
        Quaternion newRot = Quaternion.RotateTowards(angle1, angle2, Quaternion.Angle(angle1, angle2) * ratio);
        vertAngle = newRot.eulerAngles.x * Mathf.Deg2Rad;
    }
}
