using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBeeChase : AAIState
{
    [SerializeField] float radiusForChaseTemp;
    [SerializeField] float rotSpeed;

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
        return Player.position;
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
        return Vector3.Distance(transform.position, Player.position) <= radiusForChaseTemp;
    }

    public override bool ShouldFinishState()
    {
        return Vector3.Distance(transform.position, Player.position) > radiusForChaseTemp;
    }

    public override Vector2 GetRotation()
    {
        Vector2 posDiffXZ = new Vector2(Player.position.x - transform.position.x, Player.position.z - transform.position.z);
        float angle = (-Mathf.Atan2(posDiffXZ.y, posDiffXZ.x) * Mathf.Rad2Deg) + 180;
        return new Vector2(angle, rotSpeed);
    }
}
