using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles all camera movement and camera-related inputs
/// </summary>
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CameraUtils))]
public class CameraControl : UsesInputActions
{
    [SerializeField] private float pivotSensitivity;
    [SerializeField] private float pivotGravity;
    [SerializeField] private float maxPivotSpeedHoriz;
    [SerializeField] private float maxPivotSpeedVert;
    private float horizPivotSpeed = 0;
    private float vertPivotSpeed = 0;
    private PlayerInput playerInput;
    private CameraUtils camUtils;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        camUtils = GetComponent<CameraUtils>();
    }

    private void LateUpdate()
    {
        ChangeAngles();
    }

    /// <summary>
    /// Changes horizontal and vertical camera angles depending on camera-based input
    /// </summary>
    private void ChangeAngles()
    {
        // TODO smoothen input properly (actually understanding where the input comes from)
        if (playerInput.currentControlScheme == "GamePad" || true)
        {
            // Smooth input
            float horizInput = inputActions.Camera.HorizontalRotate.ReadValue<float>();
            float vertInput = inputActions.Camera.VerticalRotate.ReadValue<float>();
            horizPivotSpeed = InputUtils.SmoothedInput(horizPivotSpeed, horizInput, pivotSensitivity, pivotGravity);
            vertPivotSpeed = InputUtils.SmoothedInput(vertPivotSpeed, vertInput, pivotSensitivity, pivotGravity);
            float horizMove = horizPivotSpeed * maxPivotSpeedHoriz * Time.deltaTime;
            float vertMove = vertPivotSpeed * maxPivotSpeedVert * Time.deltaTime;
            camUtils.RotateBy(horizMove, vertMove);
        }
        else
        {
            // Don't smooth input
            horizPivotSpeed = GetOldMouseInput().x; // THIS IS THE ONLY USE OF THE OLD INPUT SYSTEM IN THE GAME. IT'S BECAUSE THE 
            vertPivotSpeed = GetOldMouseInput().y; // NEW INPUT SYSTEM WON'T SMOOTH MOUSE MOVEMENT PROPERLY.
            float horizMove = horizPivotSpeed * maxPivotSpeedHoriz * Time.deltaTime;
            float vertMove = vertPivotSpeed * maxPivotSpeedVert * Time.deltaTime;
            camUtils.RotateBy(horizMove, vertMove);
        }
    }
}
