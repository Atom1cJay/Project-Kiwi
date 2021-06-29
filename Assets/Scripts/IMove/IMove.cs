using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides access to methods for moves which can modify their contents
/// in some way.
/// </summary>
public interface IMove : IMoveImmutable
{
    /// <summary>
    /// Simulates the time moving forward for this move by exactly one
    /// (Update) frame. 
    /// </summary>
    void AdvanceTime();
}
