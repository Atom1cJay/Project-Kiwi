using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a platform which moves at the frequency of the Update() method.
/// Contains useful methods for any moving platform.
/// </summary>
public abstract class AMovingPlatform : MonoBehaviour
{
    // Event to be fired whenever the platform moves or rotates.
    public UnityEvent onTransformChange = new UnityEvent();

    /// <summary>
    /// How much did the platform move this frame?
    /// </summary>
    public abstract Vector3 MvmtThisFrame();

    /// <summary>
    /// How much did the platform rotate this frame?
    /// This does not take into account the pivot of rotation.
    /// </summary>
    public abstract Vector3 RotThisFrame();

    /// <summary>
    /// Given the position of some child object, what should the CHANGE in
    /// the child object's position be, specifically from rotation, after
    /// this frame?
    /// </summary>
    public abstract Vector3 PosChangeFromRotThisFrame(Vector3 childPos);

    /// <summary>
    /// The player has just landed on this platform. Complete any appropriate
    /// actions for this situation.
    /// </summary>
    public abstract void Register();

    /// <summary>
    /// The player has just left this platform. Complete any appropriate
    /// actions for this situation.
    /// </summary>
    public abstract void Deregister();
}
