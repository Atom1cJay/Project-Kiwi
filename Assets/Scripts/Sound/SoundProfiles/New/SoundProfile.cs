using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class capable of making sound according to a specific set of parameters
/// (defined in subclasses).
/// </summary>
public abstract class SoundProfile : ScriptableObject
{
    /// <summary>
    /// Start the "world" of this sound.
    /// </summary>
    public abstract void Initiate();

    /// <summary>
    /// Move time in the "world" of this sound forward by one frame.
    /// </summary>
    public abstract void AdvanceTime();

    /// <summary>
    /// End the "world" of this sound. If anything needs to be suddenly halted,
    /// do it here.
    /// </summary>
    public abstract void Finish();
}
