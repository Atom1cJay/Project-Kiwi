using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents information related to rotation movement, including the
/// relative speed of the rotation, along with whether or not the speed
/// of the rotation should be related to the distance to rotate.
/// </summary>
public struct RotationInfo
{
    public RotationInfo(float spd, bool spdRelativeToDist)
    {
        speed = spd;
        speedRelativeToDistance = spdRelativeToDist;
    }

    public float speed { get; }
    public bool speedRelativeToDistance { get; }
}
