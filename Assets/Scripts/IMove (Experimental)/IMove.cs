using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IMove
{
    /// <summary>
    /// Gives the horizontal speed the player should be at this frame for
    /// this move.
    /// </summary>
    /// <returns></returns>
    float GetHorizSpeedThisFrame();
}
