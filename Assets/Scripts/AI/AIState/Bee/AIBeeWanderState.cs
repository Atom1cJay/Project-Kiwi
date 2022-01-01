using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the AI state of a bee, when it is simply wandering around
/// with no target.
/// </summary>
public class AIBeeWanderState : AAIState
{
    [SerializeField] float radiusForWanderTemp;

    public override void AdvanceTime()
    {
        // Nothing
    }

    public override string GetAnimationID()
    {
        return "wander";
    }

    public override Vector3 GetGoalPos()
    {
        return transform.position;
    }

    public override void RegisterAsState()
    {
        // Nothing
    }

    public override bool DestroysEnemy()
    {
        return false;
    }

    public override bool ShouldBeginState()
    {
        return Vector3.Distance(transform.position, Player.position) > radiusForWanderTemp;
    }

    public override bool ShouldFinishState()
    {
        return Vector3.Distance(transform.position, Player.position) <= radiusForWanderTemp;
    }

    public override Vector2 GetRotation()
    {
        return Vector2.zero;
    }
}
