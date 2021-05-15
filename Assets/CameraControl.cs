using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// FIXME controller to mouse is a problem
/// <summary>
/// Handles all camera movement and camera-related inputs
/// </summary>
public class CameraControl : UsesMouseInput
{
    [Header("Basic Settings")]
    [SerializeField] private Transform target; // Object to look at / surround
    [SerializeField] private float radiusToTarget; // Distance from target
    [SerializeField] private float horizAngle = 0; // 0 = directly behind, 1.57 = to right. Initial value serialized.
    [SerializeField] private float vertAngle = 0; // 0 = exactly aligned, 1.57 = on top. Initial value serialized.
    [SerializeField] private float vertAngleMin = 0f;
    [SerializeField] private float vertAngleMax = 1.57f;
    [Header("Camera Control Settings")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] private float pivotSensitivity;
    [SerializeField] private float pivotGravity;
    [SerializeField] private float maxPivotSpeedHoriz;
    [SerializeField] private float maxPivotSpeedVert;
    [Header("Auto Rotation Settings")] // TODO move to different script
    [SerializeField] private float minAutoInput;
    [SerializeField] private float autoSensitivity;
    [SerializeField] private float autoGravity;
    [SerializeField] private float maxAutoSpeed;
    private float horizPivotSpeed = 0;
    private float vertPivotSpeed = 0;
    private float horizAutoRotSpeed = 0;

    private void LateUpdate()
    {
        ChangeAnglesAuto();
        ChangeAnglesManual();
        DetermineTransform();
    }

    private void ChangeAnglesAuto()
    {
        float autoInput = inputActions.Player.Move.ReadValue<Vector2>().x;
        if (Mathf.Abs(autoInput) < minAutoInput) { autoInput = 0; }
        horizAutoRotSpeed = InputUtils.SmoothedInput(horizAutoRotSpeed, autoInput, autoSensitivity, autoGravity);
        horizAngle -= horizAutoRotSpeed * maxAutoSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Changes horizontal and vertical camera angles depending on camera-based input
    /// </summary>
    private void ChangeAnglesManual()
    {
        if (playerInput.currentControlScheme == "GamePad")
        {
            // Smooth input
            float horizInput = inputActions.Camera.HorizontalRotate.ReadValue<float>();
            float vertInput = inputActions.Camera.VerticalRotate.ReadValue<float>();
            horizPivotSpeed = InputUtils.SmoothedInput(horizPivotSpeed, horizInput, pivotSensitivity, pivotGravity);
            vertPivotSpeed = InputUtils.SmoothedInput(vertPivotSpeed, vertInput, pivotSensitivity, pivotGravity);
            horizAngle += horizPivotSpeed * maxPivotSpeedHoriz * Time.deltaTime;
            vertAngle += vertPivotSpeed * maxPivotSpeedVert * Time.deltaTime;
            vertAngle = Mathf.Clamp(vertAngle, vertAngleMin, vertAngleMax);
        }
        else
        {
            // Don't smooth input
            horizPivotSpeed = GetOldMouseInput().x; // THIS IS THE ONLY USE OF THE OLD INPUT SYSTEM IN THE GAME. IT'S BECAUSE THE 
            vertPivotSpeed = GetOldMouseInput().y; // NEW INPUT SYSTEM WON'T SMOOTH MOUSE MOVEMENT PROPERLY.
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
