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
    [SerializeField] private float mouseRotateMultiplier;
    [SerializeField] LayerMask playerVisuals;

    private float horizPivotSpeed = 0;
    private float vertPivotSpeed = 0;
    private CameraUtils camUtils;
    float InvertX, InvertY;
    float userSensitivitySetting;

    private void Awake()
    {
        camUtils = GetComponent<CameraUtils>();
        userSensitivitySetting = PlayerPrefsWhisperer.GetMouseSensitivity();
    }

    private void OnEnable()
    {
        PlayerPrefsWhisperer.OnCameraSensitivityChanged.AddListener(OnUserSensitivityChanged);
    }

    private void OnDisable()
    {
        PlayerPrefsWhisperer.OnCameraSensitivityChanged.RemoveListener(OnUserSensitivityChanged);
    }

    void OnUserSensitivityChanged(float sensitivity)
    {
        userSensitivitySetting = sensitivity;
    }

    private void Start()
    {
        iah.inputActions.Camera.AutoAdjust.performed += _ => AutoAdjustToBack();
        PlayerPrefsWhisperer.OnCameraInversionToggled.AddListener(() => ReadInversionValues());
        ReadInversionValues();
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
        float horizMove = horizPivotSpeed * maxPivotSpeedHoriz * userSensitivitySetting * Time.deltaTime;
        float vertMove = vertPivotSpeed * maxPivotSpeedVert * userSensitivitySetting * Time.deltaTime;
        camUtils.RotateBy(horizMove * InvertX, vertMove * InvertY);
        // Keyboard
        float horizPivotSpeedMouse = iah.GetOldMouseInput().x * mouseRotateMultiplier; // THIS IS THE ONLY USE OF THE OLD INPUT SYSTEM IN THE GAME. IT'S BECAUSE THE
        float vertPivotSpeedMouse = iah.GetOldMouseInput().y * mouseRotateMultiplier; // NEW INPUT SYSTEM WON'T SMOOTH MOUSE MOVEMENT PROPERLY.
        horizMove = horizPivotSpeedMouse * maxPivotSpeedHoriz * userSensitivitySetting;
        vertMove = vertPivotSpeedMouse * maxPivotSpeedVert * userSensitivitySetting;
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
    /// Based on PlayerPrefs, decides whether to invert the camera on the X axis.
    /// </summary>
    void ReadInversionValues()
    {
        InvertX = PlayerPrefsWhisperer.GetCameraXInverted() ? -1 : 1;
        InvertY = PlayerPrefsWhisperer.GetCameraYInverted() ? -1 : 1;
    }


    /// <summary>
    /// Renders the player visuals
    /// </summary>
    public void renderPlayer(bool render)
    {
        if (render)
            GetComponent<Camera>().cullingMask |= playerVisuals;
        else
            GetComponent<Camera>().cullingMask &= ~playerVisuals;
    }
}
