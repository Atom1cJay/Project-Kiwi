using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an AI move which can be simulated and transitioned into
/// using a behavior-tree-esque structure.
/// </summary>
public interface IAIState
{
    /// <summary>
    /// Should this state be transitioned to, given the current state of the game?
    /// </summary>
    public bool ShouldTransition();

    /// <summary>
    /// Advances time in the "simulation" of this AI state by one frame.
    /// </summary>
    public void AdvanceTime();

    /// <summary>
    /// What should the velocity of the AI be in the X, Y, and Z axes?
    /// This method should only be called/used after this move has been transitioned
    /// into.
    /// </summary>
    public Vector3 GetVelocity();

    /// <summary>
    /// What is the name of the animation that should be playing right now?
    /// This method should only be called/used after this move has been transitioned
    /// into.
    /// </summary>
    public string GetAnimationID();

    /// <summary>
    /// Should this move destroy the enemy?
    /// Note that, if this is the case, any future moves should be ignored.
    /// </summary>
    public abstract bool ShouldDestroy();
}
