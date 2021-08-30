using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The "control panel" through which orders related to camera movement are handled.
/// </summary>
[RequireComponent(typeof(CameraUtils))]
public class CameraControl : MonoBehaviour
{
    [SerializeField] InputActionsHolder iah;
    [SerializeField] private float pivotSensitivity;
    [SerializeField] private float pivotGravity;
    [SerializeField] private float maxPivotSpeedHoriz;
    [SerializeField] private float maxPivotSpeedVert;
    [SerializeField] private float autoAdjustTime;
    private float horizPivotSpeed = 0;
    private float vertPivotSpeed = 0;
    private CameraUtils camUtils;
    float InvertX, InvertY;

    private void Awake()
    {
        camUtils = GetComponent<CameraUtils>();
    }

    private void Start()
    {
        iah.inputActions.Camera.AutoAdjust.performed += _ => AutoAdjustToBack();
    }

    /// <summary>
    /// Changes horizontal and vertical camera angles depending on camera-based input
    /// </summary>
    public void HandleManualControl()
    {
        // Controller
        float horizInput = iah.inputActions.Camera.HorizontalRotate.ReadValue<float>();
        float vertInput = iah.inputActions.Camera.VerticalRotate.ReadValue<float>();
        horizPivotSpeed = InputUtils.SmoothedInput(horizPivotSpeed, horizInput, pivotSensitivity, pivotGravity);
        vertPivotSpeed = InputUtils.SmoothedInput(vertPivotSpeed, vertInput, pivotSensitivity, pivotGravity);
        float horizMove = horizPivotSpeed * maxPivotSpeedHoriz * Time.deltaTime;
        float vertMove = vertPivotSpeed * maxPivotSpeedVert * Time.deltaTime;
        camUtils.RotateBy(horizMove * InvertX, vertMove * InvertY);
        // Keyboard
        float horizPivotSpeedMouse = iah.GetOldMouseInput().x; // THIS IS THE ONLY USE OF THE OLD INPUT SYSTEM IN THE GAME. IT'S BECAUSE THE
        float vertPivotSpeedMouse = iah.GetOldMouseInput().y; // NEW INPUT SYSTEM WON'T SMOOTH MOUSE MOVEMENT PROPERLY.
        horizMove = horizPivotSpeedMouse * maxPivotSpeedHoriz * Time.deltaTime;
        vertMove = vertPivotSpeedMouse * maxPivotSpeedVert * Time.deltaTime;
        camUtils.RotateBy(horizMove * InvertX, vertMove * InvertY);
    }

    void AutoAdjustToBack()
    {
        camUtils.RotateToTargetY(autoAdjustTime);
    }

    /// <summary>
    /// Moves the camera's horizontal angle to face the back of the player,
    /// with the amount of progress this frame being the given ratio.
    /// </summary>
    public void AdjustToBack(float ratio)
    {
        camUtils.RotateToBackBy(ratio);
    }

    /// <summary>
    /// Moves the camera to the given vertical angle (degrees), with the amount of progress
    /// this frame being the given ratio.
    /// </summary>
    public void AdjustVertical(float ratio, float angle)
    {
        camUtils.RotateToVertAngle(ratio, angle);
    }

    /// <summary>
    /// Sends camera instructions for CameraUtils to execute.
    /// </summary>
    public void TakeInstructions(ACameraInstruction instructions)
    {
        camUtils.HandleInstructions(instructions);
    }

    /// <summary>
    /// Sets the X inversion constant based on whether X should be inverted.
    /// </summary>
    public void SetXInverted(bool inverted)
    {
        InvertX = inverted ? -1 : 1;
    }

    /// <summary>
    /// Sets the Y inversion constant based on whether Y should be inverted.
    /// </summary>
    public void SetYInverted(bool inverted)
    {
        InvertY = inverted ? -1 : 1;
    }
}
