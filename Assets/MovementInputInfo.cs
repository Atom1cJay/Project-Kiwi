using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInputInfo : UsesInputActions
{
    /// <summary>
    /// Gives the normalized horizontal movement input.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetHorizontalInput()
    {
        Vector2 rawInput = inputActions.Player.Move.ReadValue<Vector2>();

        if (rawInput.magnitude > 1)
        {
            rawInput = rawInput.normalized;
        }

        print(rawInput);
        return rawInput;
    }
}
