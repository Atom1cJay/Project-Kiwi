using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains helpful methods related to input
/// </summary>
public static class InputUtils
{
    /// <summary>
    /// Modifies the current input by one frame's worth of smoothened change
    /// </summary>
    /// <param name="currentValue">The smoothed input value as it is currently.</param>
    /// <param name="rawInput">A value, between -1 and 1, which describes the raw input this frame.</param>
    /// <param name="sensitivity">How fast the smoothed input value moves to its target value</param>
    /// <param name="gravity">How fast the input diminishes when no (or counter) input is present</param>
    /// <returns>A float between -1 and 1</returns>
    public static float SmoothedInput(float currentValue, float rawInput, float sensitivity, float gravity)
    {
        float newValue;

        bool movingTowardsZero = Mathf.Abs(rawInput) < Mathf.Abs(currentValue) || Mathf.Sign(rawInput) != Mathf.Sign(currentValue);
        if (movingTowardsZero)
        {
            // Using gravity
            newValue = Mathf.MoveTowards(currentValue, rawInput, gravity * Time.deltaTime);
        }
        else
        {
            // Using sensitivity
            newValue = Mathf.MoveTowards(currentValue, rawInput, sensitivity * Time.deltaTime);
        }

        return newValue;
    }
}
