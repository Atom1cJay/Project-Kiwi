using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a platform which moves on the Update() method, and can be ridden
/// on by the player.
/// </summary>
public abstract class AMovingPlatform : MonoBehaviour
{
    // Optional Pre-Move Housekeeping
    public virtual void FrameStart() { }

    public abstract void Translate();

    public abstract void Rotate();
}
