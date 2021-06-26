using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles all camera movement and camera-related inputs
/// </summary>
[RequireComponent(typeof(CameraUtils))]
public class CameraControl : UsesInputActions
{
    [SerializeField] private float pivotSensitivity;
    [SerializeField] private float pivotGravity;
    [SerializeField] private float maxPivotSpeedHoriz;
    [SerializeField] private float maxPivotSpeedVert;
    private float horizPivotSpeed = 0;
    private float vertPivotSpeed = 0;
    private CameraUtils camUtils;

    private void Start()
    {
        camUtils = GetComponent<CameraUtils>();
    }

    private void Update()
    {
        ChangeAngles();
    }

    /// <summary>
    /// Changes horizontal and vertical camera angles depending on camera-based input
    /// </summary>
    private void ChangeAngles()
    {
        // Controller
        float horizInput = inputActions.Camera.HorizontalRotate.ReadValue<float>();
        float vertInput = inputActions.Camera.VerticalRotate.ReadValue<float>();
        horizPivotSpeed = InputUtils.SmoothedInput(horizPivotSpeed, horizInput, pivotSensitivity, pivotGravity);
        vertPivotSpeed = InputUtils.SmoothedInput(vertPivotSpeed, vertInput, pivotSensitivity, pivotGravity);
        float horizMove = horizPivotSpeed * maxPivotSpeedHoriz * Time.deltaTime;
        float vertMove = vertPivotSpeed * maxPivotSpeedVert * Time.deltaTime;
        camUtils.RotateBy(horizMove, vertMove);
        // Keyboard
        if (GetOldMouseInput().x != 0 || GetOldMouseInput().y != 0)
        {
            horizPivotSpeed = GetOldMouseInput().x; // THIS IS THE ONLY USE OF THE OLD INPUT SYSTEM IN THE GAME. IT'S BECAUSE THE 
            vertPivotSpeed = GetOldMouseInput().y; // NEW INPUT SYSTEM WON'T SMOOTH MOUSE MOVEMENT PROPERLY.
            horizMove = horizPivotSpeed * maxPivotSpeedHoriz * Time.deltaTime;
            vertMove = vertPivotSpeed * maxPivotSpeedVert * Time.deltaTime;
            camUtils.RotateBy(horizMove, vertMove);
        }
    }
}
