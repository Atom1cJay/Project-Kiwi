using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class providing helpful resources for any AIState class.
/// Also allows any classes implementing IAIState to be serialized.
/// </summary>
public abstract class AAIState : IAIState
{
    // REMINDER: FOR ALL STATIC VARIABLES REMEMBER TO MAKE THEM RESET PROPERLY ON REPLAY

    public abstract void AdvanceTime();

    public abstract string GetAnimationID();

    public abstract Vector3 GetVelocity();

    public abstract bool ShouldTransition();

    public abstract bool ShouldDestroy();
}
