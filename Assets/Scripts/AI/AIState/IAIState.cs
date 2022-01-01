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
    /// Is the game in a state where it would make sense for this move to begin?
    /// </summary>
    public bool ShouldBeginState();

    /// <summary>
    /// Is the game in a state where it would make sense for this move to end?
    /// </summary>
    public bool ShouldFinishState();

    /// <summary>
    /// This state is now the current state of the enemy. Do anything that needs
    /// to be done at the start of the state.
    /// </summary>
    public void RegisterAsState();

    /// <summary>
    /// Advances time in the "simulation" of this AI state by one frame.
    /// </summary>
    public void AdvanceTime();

    /// <summary>
    /// What position does the AI want to go to?
    /// </summary>
    public Vector3 GetGoalPos();

    /// <summary>
    /// How should the character rotate? This method returns a Vector2, WHERE
    /// THE X FIELD REPRESENTS THE ROTATION GOAL IN DEGREES, AND THE Y FIELD
    /// REPRESENTS THE SPEED (IN DEGREES/SEC) IN WHICH THAT GOAL SHOULD BE APPROACHED.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetRotation();

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
    public abstract bool DestroysEnemy();
}
