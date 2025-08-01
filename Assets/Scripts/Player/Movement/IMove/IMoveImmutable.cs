﻿using System;
using System.Collections;
using System.Collections.Generic;
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
    /// Gives the rotation info related to the player this
    /// frame.
    /// </summary>
    RotationInfo GetRotationInfo();

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
    /// Gives this move as a String with no spaces, all in lowercase.
    /// </summary>
    /// <returns></returns>
    string AsString();

    /// <summary>
    /// What move should start being performed next frame?
    /// </summary>
    IMove GetNextMove();

    /// <summary>
    /// Gives the attacks (if any) associated with the move. If there is none,
    /// returns null.
    /// </summary>
    Attack[] GetAttack();

    /// <summary>
    /// Returns the particle information for all particles that should be
    /// spawned **this frame**.
    /// </summary>
    MovementParticleInfo.MovementParticles[] GetParticlesToSpawn();

    /// <summary>
    /// Returns the particle information for all particles that should be
    /// stopped **this frame**.
    /// If no such particles exist, or the particles are already stopped,
    /// nothing will happen.
    /// </summary>
    MovementParticleInfo.MovementParticles[] GetParticlesToStop();

    /// <summary>
    /// Should a pause be allowed to occur if a pause input is made by the player?
    /// </summary>
    bool Pausable();
}
