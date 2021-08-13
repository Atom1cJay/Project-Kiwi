﻿using System;
using UnityEngine;

/// <summary>
/// Provides access to an IMove object, without the ability to mutate its
/// contents or modify its state in any way.
/// </summary>
public interface IMoveImmutable
{
    /// <summary>
    /// Gives the horizontal speed the player should be at this frame for
    /// this move, based on the current simulated time this frame.
    /// </summary>
    Vector2 GetHorizSpeedThisFrame();

    /// <summary>
    /// Gives the vertical speed the player should be at this frame for
    /// this move, based on the current simulated time in this move.
    /// </summary>
    float GetVertSpeedThisFrame();

    /// <summary>
    /// Gives the rotation speed at which the player should be moving this
    /// frame.
    /// </summary>
    float GetRotationSpeed();

    /// <summary>
    /// Determines whether this move being initiated should bring up the triple
    /// jump counter.
    /// </summary>
    bool IncrementsTJcounter();

    /// <summary>
    /// Determines whether a triple jump cycle, if it is currently happening,
    /// should be broken. (In other words, should the triple jump count
    /// be reset right now?)
    /// </summary>
    bool TJshouldBreak();

    /// <summary>
    /// When using this move, should the movement be done in the direction
    /// of the slope that the player is standing on (or last standed on)?
    /// Otherwise, movement will always be straight.
    /// It is safest to choose true for this unless there is some special
    /// reason not to.
    /// </summary>
    bool AdjustToSlope();

    /// <summary>
    /// Should the the player rotate relative to their current positioning
    /// (true), or should they rotate TOWARDS the direction of the stick
    /// (false)?
    /// </summary>
    bool RotationIsRelative();

    /// <summary>
    /// Gives this move as a String with no spaces, all in lowercase.
    /// </summary>
    /// <returns></returns>
    string AsString();

    /// <summary>
    /// What move should start being performed next frame?
    /// </summary>
    IMove GetNextMove();

    /// <summary>
    /// What ratio, between the current camera angle and its target
    /// (at the back of the player, at a certain vertical angle),
    /// should the camera move by every frame? If none at all, returns 0.
    /// </summary>
    float CameraRotateTowardsRatio();

    /// <summary>
    /// Gives the angle (degrees) on the X axis which the camera should target. This
    /// method only applies to those moves whose CameraRotateTowardsRatio
    /// is not zero.
    /// </summary>
    float CameraVerticalAutoTarget();
}