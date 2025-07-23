using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBeeWindUp : AAIState
{
    [SerializeField] float rotSpeed;
    [SerializeField] float timeForState;
    [SerializeField] float speed;
    float timeElapsed;

    public override void AdvanceTime()
    {
        timeElapsed += Time.deltaTime;
    }

    public override string GetAnimationID()
    {
        return "chase";
    }

    public override Vector3 GetGoalPos()
    {
        Vector3 posDiff = Player.position - transform.position;
        return transform.position - posDiff.normalized;
    }

    public override float GetSpeed()
    {
        return speed;
    }

    public override void RegisterAsState()
    {
        timeElapsed = 0;
    }

    public override bool DestroysEnemy()
    {
        return false;
    }

    public override bool ShouldBeginState()
    {
        return true;
    }

    public override bool ShouldFinishState()
    {
        return timeElapsed > timeForState;
    }

    public override Vector2 GetRotation()
    {
        Vector2 posDiffXZ = new Vector2(Player.position.x - transform.position.x, Player.position.z - transform.position.z);
        float angle = (-Mathf.Atan2(posDiffXZ.y, posDiffXZ.x) * Mathf.Rad2Deg) + 180;
        return new Vector2(angle, rotSpeed);
    }
}
