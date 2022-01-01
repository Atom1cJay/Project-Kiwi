using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class providing helpful resources for any AIState class.
/// Also allows any classes implementing IAIState to be serialized.
/// </summary>
public abstract class AAIState : MonoBehaviour, IAIState
{
    // REMINDER: FOR ALL STATIC VARIABLES REMEMBER TO MAKE THEM RESET PROPERLY ON REPLAY
    private static Transform player;

    public static Transform Player
    {
        get
        {
            if (player != null) { return player; }
            else
            {
                player = GameObject.Find("Player").transform;
                return player;
            }
        }
    }

    public abstract void AdvanceTime();

    public abstract void RegisterAsState();

    public abstract string GetAnimationID();

    public abstract Vector3 GetGoalPos();

    public abstract Vector2 GetRotation();

    public abstract bool ShouldBeginState();

    public abstract bool ShouldFinishState();

    public abstract bool DestroysEnemy();
}
