using System;
using UnityEngine;

/// <summary>
/// Represents the dive move in the air
/// </summary>
public class Dive : IMove
{
    private readonly float diveSpeed;

    /// <summary>
    /// Constructs an instance of the dive move class.
    /// </summary>
    /// <param name="diveSpeed">The serialized speed at which the
    /// player is expected to dive.</param>
    public Dive(float diveSpeed)
    {
        if (diveSpeed < 0)
        {
            Debug.LogError("Dive speed being less than 0 does not make sense.");
        }

        this.diveSpeed = diveSpeed;
    }

    public float GetHorizSpeedThisFrame()
    {
        return diveSpeed;
    }
}
