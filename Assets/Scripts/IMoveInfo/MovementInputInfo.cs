using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a container and calculator for information relevant to
/// the input of the user.
/// </summary>
public class MovementInputInfo : MonoBehaviour
{
    [SerializeField] InputActionsHolder usesInputActions;
    public bool JumpInputPending { get; private set; }
    public bool JumpCancelInputPending { get; private set; }
    public bool DiveInputPending { get; private set; }
    public bool HorizAirBoostChargeInputPending { get; private set; }
    public bool HorizAirBoostReleaseInputPending { get; private set; }
    public bool VertAirBoostChargeInputPending { get; private set; }
    public bool VertAirBoostReleaseInputPending { get; private set; }
    public float HorizBoostInput { get; private set; }
    public float VertBoostInput { get; private set; }

    /// <summary>
    /// Gives the InputActions instance being used to calculate
    /// input information on the player.
    /// </summary>
    /// <returns></returns>
    public InputActions GetInputActions()
    {
        return usesInputActions.inputActions;
    }

    /// <summary>
    /// Gives the normalized horizontal movement input.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetHorizontalInput()
    {
        Vector2 rawInput = usesInputActions.inputActions.Player.Move.ReadValue<Vector2>();

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
