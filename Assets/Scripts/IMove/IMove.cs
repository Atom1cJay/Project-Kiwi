using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    /// <summary>
    /// Gives the horizontal speed the player should be at this frame for
    /// this move. Assumed that this method is called every (update) frame while
    /// the move is active.
    /// </summary>
    /// <returns></returns>
    float GetHorizSpeedThisFrame();

    /// <summary>
    /// Gives the vertical speed the player should be at this frame for
    /// this move. Assumed that this method is called every (update) frame while
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
    /// Gives this move as a String with no spaces, all in lowercase
    /// </summary>
    /// <returns></returns>
    string asString();
}
