using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBeeChase : AAIState
{
    Vector3 goalPos;
    [SerializeField] float radiusForChaseTemp;

    public override void AdvanceTime()
    {
        // Nothing
    }

    public override string GetAnimationID()
    {
        return "chase";
    }

    public override Vector3 GetGoalPos()
    {
        return goalPos;
    }

    public override void RegisterAsState()
    {
        goalPos = Player.position;
    }

    public override bool DestroysEnemy()
    {
        return false;
    }

    public override bool ShouldBeginState()
    {
        return Vector3.Distance(transform.position, Player.position) <= radiusForChaseTemp;
    }

    public override bool ShouldFinishState()
    {
        return Vector3.Distance(transform.position, Player.position) > radiusForChaseTemp;
    }

    public override Vector2 GetRotation()
    {
        return Vector2.zero;
    }
}
