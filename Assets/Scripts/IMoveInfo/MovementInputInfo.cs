using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a container and calculator for information relevant to
/// the input of the user.
/// </summary>
public class MovementInputInfo : MonoBehaviour
{
    [SerializeField] InputActionsHolder inputActionsHolder;
    [SerializeField] GameObject relevantCamera;
    public bool JumpInputPending { get; private set; }
    public bool JumpCancelInputPending { get; private set; }
    public bool DiveInputPending { get; private set; }
    public bool HorizAirBoostChargeInputPending { get; private set; }
    public bool HorizAirBoostReleaseInputPending { get; private set; }
    public bool VertAirBoostChargeInputPending { get; private set; }
    public bool VertAirBoostReleaseInputPending { get; private set; }
    public float HorizBoostInput { get; private set; }
    public float VertBoostInput { get; private set; }

    [HideInInspector] public UnityEvent OnJump;
    [HideInInspector] public UnityEvent OnJumpCancelled;
    [HideInInspector] public UnityEvent OnDiveInput;
    [HideInInspector] public UnityEvent OnHorizBoostCharge;
    [HideInInspector] public UnityEvent OnHorizBoostRelease;
    [HideInInspector] public UnityEvent OnVertBoostCharge;
    [HideInInspector] public UnityEvent OnVertBoostRelease;

    private MovementSettingsSO movementSettings;

    void Start()
    {
        movementSettings = MovementSettingsSO.Instance;
        inputActionsHolder.inputActions.Player.Jump.performed += _ => OnJump.Invoke();
        inputActionsHolder.inputActions.Player.Jump.canceled += _ => OnJumpCancelled.Invoke();
        inputActionsHolder.inputActions.Player.Boost.started += _ => OnHorizBoostCharge.Invoke();
        inputActionsHolder.inputActions.Player.VertBoost.started += _ => OnVertBoostCharge.Invoke();
        inputActionsHolder.inputActions.Player.Boost.canceled += _ => OnHorizBoostRelease.Invoke();
        inputActionsHolder.inputActions.Player.VertBoost.canceled += _ => OnVertBoostRelease.Invoke();
        inputActionsHolder.inputActions.Player.Dive.performed += _ => OnDiveInput.Invoke();
    }

    /// <summary>
    /// Gives the InputActions instance being used to calculate
    /// input information on the player.
    /// </summary>
    public InputActions GetInputActions()
    {
        return inputActionsHolder.inputActions;
    }

    /// <summary>
    /// Gives the normalized horizontal movement input.
    /// </summary>
    public Vector2 GetHorizontalInput()
    {
        Vector2 rawInput = inputActionsHolder.inputActions.Player.Move.ReadValue<Vector2>();

        if (rawInput.magnitude > 1)
        {
            rawInput = rawInput.normalized;
        }

        return rawInput;
    }

    /// <summary>
    /// Is the horizontal input dissonance high enough for an air reverse to be registered?
    /// </summary>
    public bool AirReverseInput()
    {
        return GetHorizDissonance() > movementSettings.DissonanceForAirReverse;
    }

    /// <summary>
    /// Is the horizontal input dissonance high enough for a hard turn to be registered?
    /// </summary>
    public bool HardTurnInput()
    {
        return GetHorizDissonance() > movementSettings.DissonanceForHardTurn;
    }

    /// <summary>
    /// Gives the distance (min = 0, max = PI) between the direction the player is facing and the
    /// direction of horizontal input
    /// </summary>
    public float GetHorizDissonance()
    {
        Vector2 rawInput = GetHorizontalInput();
        float camDirection = relevantCamera.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float directionFacing = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float inputDirection = Mathf.Atan2(rawInput.x, rawInput.y) + camDirection;
        float inputVsFacing = Mathf.PI - Mathf.Abs(Mathf.Abs((inputDirection - directionFacing) % (2 * Mathf.PI)) - Mathf.PI);
        return inputVsFacing;
    }
}
