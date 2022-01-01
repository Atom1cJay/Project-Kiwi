using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the AI state of a bee when it notices the player.
/// </summary>
public class AIBeeNotice : AAIState
{
    float timePassedSinceStateStart;
    [SerializeField] float radiusForChaseTemp;
    [SerializeField] float timeForState;
    float noticeAngle;
    [SerializeField] float rotationSpeed; // Radians

    public override void AdvanceTime()
    {
        timePassedSinceStateStart += Time.deltaTime;
    }

    public override bool DestroysEnemy()
    {
        return false;
    }

    public override string GetAnimationID()
    {
        return "notice";
    }

    public override Vector3 GetGoalPos()
    {
        return transform.position;
    }

    public override void RegisterAsState()
    {
        timePassedSinceStateStart = 0;
        Vector2 posDiffXZ = new Vector2(Player.position.x - transform.position.x, Player.position.z - transform.position.z);
        noticeAngle = (-Mathf.Atan2(posDiffXZ.y, posDiffXZ.x) * Mathf.Rad2Deg) + 180;
    }

    public override bool ShouldBeginState()
    {
        return Vector3.Distance(transform.position, Player.position) <= radiusForChaseTemp;
    }

    public override bool ShouldFinishState()
    {
        return timePassedSinceStateStart >= timeForState;
    }

    public override Vector2 GetRotation()
    {
        return new Vector2(noticeAngle, rotationSpeed);
    }
}
