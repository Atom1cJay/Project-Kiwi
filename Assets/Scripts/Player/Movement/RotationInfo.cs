using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents information related to rotation movement, including the
/// angular speed of the rotation, along with whether the rotation should
/// stop once it reaches the direction of input.
/// </summary>
public struct RotationInfo
{
    /// <summary>
    /// Constructs a RotationInfo, including the angular speed of rotation,
    /// and whether the rotation should stop once it reaches the direction
    /// of input.
    /// </summary>
    /// <param name="spd">The speed of rotation (degrees)</param>
    /// <param name="spdRelativeToDist">Whether the rotation should stop
    /// once the direction of input is reached</param>
    public RotationInfo(float spd, bool spdRelativeToDist)
    {
        speed = spd;
        speedRelativeToDistance = spdRelativeToDist;
    }

    public float speed { get; }
    public bool speedRelativeToDistance { get; }
}
