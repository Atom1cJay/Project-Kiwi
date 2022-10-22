using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraUtils))]
public class CameraAutoRotator : MonoBehaviour
{
    [SerializeField] private float autoSensitivity;
    [SerializeField] private float autoGravity;
    [SerializeField] private float autoSpeedMultiplier;
    [SerializeField] private float minPlayerXSpeed;
    [SerializeField] MovementInputInfo mii;
    [SerializeField] MoveExecuter me;
    CameraUtils camUtils;
    float myXSpeed;

    private void Awake()
    {
        camUtils = GetComponent<CameraUtils>();
    }

    private void Update()
    {
        // Set up transform.right
        Vector3 transformRight = transform.right;
        transformRight.y = 0;
        transformRight = transformRight.normalized;
        // Figure out player speed relative to transform.right
        Vector2 playerSpeed = MoveExecuter.GetCurrentMove().GetHorizSpeedThisFrame();
        Vector3 playerSpeedXZ = new Vector3(playerSpeed.x, 0, playerSpeed.y);
        float xSpeedRelative = Vector3.Dot(playerSpeedXZ.normalized, transformRight) * playerSpeed.magnitude;
        // Reset the tracked speed if it's below some minimum
        if (Mathf.Abs(xSpeedRelative) <= minPlayerXSpeed)
        {
            xSpeedRelative = 0;
        }
        myXSpeed = InputUtils.SmoothedInput(myXSpeed, xSpeedRelative * Mathf.Pow(Mathf.Cos(Vector3.Angle(playerSpeedXZ, transformRight) * Mathf.Deg2Rad), 2) * autoSpeedMultiplier, autoSensitivity, autoGravity);
        camUtils.RotateBy(-myXSpeed * Time.deltaTime, 0);
    }
}
