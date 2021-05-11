using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles all camera movement and camera-related inputs
/// </summary>
public class CameraControl : UsesInputActions
{
    [SerializeField] private Transform target; // Object to look at / surround
    [SerializeField] private float radiusToTarget; // Distance from target
    [SerializeField] private float pivotSensitivity;
    [SerializeField] private float pivotGravity;
    [SerializeField] private float maxPivotSpeedHoriz;
    [SerializeField] private float maxPivotSpeedVert;
    [SerializeField] private float horizAngle = 0; // 0 = directly behind, 1.57 = to right
    [SerializeField] private float vertAngle = 0; // 0 = exactly aligned, 1.57 = on top
    [SerializeField] private float vertAngleMin = 0f;
    [SerializeField] private float vertAngleMax = 1.57f;
    [SerializeField] PlayerInput playerInput;

    private void LateUpdate()
    {
        ChangeAngles();
        DetermineTransform();
    }

    // To be used in the below method only
    private float horizPivotSpeed = 0;
    private float vertPivotSpeed = 0;

    /// <summary>
    /// Changes horizontal and vertical camera angles depending on any input
    /// </summary>
    private void ChangeAngles()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerInput.currentControlScheme == "GamePad")
        {
            // Smooth input
            Vector2 rawInput = inputActions.Camera.Pivot.ReadValue<Vector2>();
            horizPivotSpeed = InputUtils.SmoothedInput(horizPivotSpeed, rawInput.x, pivotSensitivity, pivotGravity);
            vertPivotSpeed = InputUtils.SmoothedInput(vertPivotSpeed, rawInput.y, pivotSensitivity, pivotGravity);
            horizAngle += horizPivotSpeed * maxPivotSpeedHoriz * Time.deltaTime;
            vertAngle += vertPivotSpeed * maxPivotSpeedVert * Time.deltaTime;
            vertAngle = Mathf.Clamp(vertAngle, vertAngleMin, vertAngleMax);
        }
        else
        {
            // Don't smooth input
            Vector2 rawInput = Mouse.current.delta.ReadUnprocessedValue();
            horizPivotSpeed = rawInput.x;
            vertPivotSpeed = rawInput.y;
            horizAngle += horizPivotSpeed * maxPivotSpeedHoriz * Time.deltaTime;
            vertAngle += vertPivotSpeed * maxPivotSpeedVert * Time.deltaTime;
            vertAngle = Mathf.Clamp(vertAngle, vertAngleMin, vertAngleMax);
        }
    }

    /// <summary>
    /// Positions and rotates the camera based on the current angles
    /// </summary>
    private void DetermineTransform()
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
        transform.LookAt(target); // TODO change
    }
}
