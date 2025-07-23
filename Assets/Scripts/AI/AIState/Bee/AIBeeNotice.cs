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

    public override float GetSpeed()
    {
        return 0;
    }

    public override void RegisterAsState()
    {
        timePassedSinceStateStart = 0;
    }

    public override bool ShouldBeginState()
    {
        Vector3 playerPosLeveled = Player.position;
        playerPosLeveled.y = transform.position.y;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (Player.position - transform.position).normalized, out hit, radiusForChaseTemp))
        {
            return hit.collider.gameObject.layer == 9 && !Physics.Raycast(transform.position, (playerPosLeveled - transform.position).normalized, radiusForChaseTemp, ~(1 << 9));
        }
        return false;
    }

    public override bool ShouldFinishState()
    {
        return timePassedSinceStateStart >= timeForState;
    }

    public override Vector2 GetRotation()
    {
        Vector2 posDiffXZ = new Vector2(Player.position.x - transform.position.x, Player.position.z - transform.position.z);
        float noticeAngle = (-Mathf.Atan2(posDiffXZ.y, posDiffXZ.x) * Mathf.Rad2Deg) + 180;
        return new Vector2(noticeAngle, rotationSpeed);
    }
}
