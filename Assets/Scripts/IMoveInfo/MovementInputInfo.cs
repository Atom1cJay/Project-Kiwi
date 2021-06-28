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

    void Start()
    {
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
    /// <returns></returns>
    public InputActions GetInputActions()
    {
        return inputActionsHolder.inputActions;
    }

    /// <summary>
    /// Gives the normalized horizontal movement input.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetHorizontalInput()
    {
        Vector2 rawInput = inputActionsHolder.inputActions.Player.Move.ReadValue<Vector2>();

        if (rawInput.magnitude > 1)
        {
            rawInput = rawInput.normalized;
        }

        return rawInput;
    }

    public bool IsAirReversing()
    {
        return false; // todo stub
    }

    public bool IsHardTurning()
    {
        return false; // todo stub
    }
}
