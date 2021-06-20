using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    /// <summary>
    /// Gives the horizontal speed the player should be at this frame for
    /// this move. Assumed that this method is called every frame while
    /// the move is active.
    /// </summary>
    /// <returns></returns>
    float GetHorizSpeedThisFrame();

    /// <summary>
    /// Gives the vertical speed the player should be at this frame for
    /// this move. Assumed that this method is called every frame while
    /// the move is active.
    /// </summary>
    /// <returns></returns>
    float GetVertSpeedThisFrame();

    /// <summary>
    /// What move should start being performed next frame?
    /// </summary>
    /// <returns></returns>
    IMove GetNextMove();

    /// <summary>
    /// Returns the move in string form. Strings are all
    /// lowercase and have no spaces in them, but other than that, they're
    /// identical to their class names.
    /// </summary>
    /// <returns></returns>
    string ToString();
}
