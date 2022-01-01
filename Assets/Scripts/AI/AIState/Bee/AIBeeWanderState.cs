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
    [SerializeField] float maxLengthOfEachMove;
    [SerializeField] float maxTimeForEachMove;
    [SerializeField] float speed;
    [SerializeField] float rotSpeed;
    Vector3 goalPos;
    float timeElapsedInMove;

    public override void AdvanceTime()
    {
        timeElapsedInMove += Time.deltaTime;
        if (timeElapsedInMove > maxTimeForEachMove || transform.position == goalPos)
        {
            StartMove();
        }
    }

    public override string GetAnimationID()
    {
        return "wander";
    }

    public override Vector3 GetGoalPos()
    {
        return goalPos;
    }

    public override float GetSpeed()
    {
        return speed;
    }

    public override void RegisterAsState()
    {
        StartMove();
    }

    void StartMove()
    {
        timeElapsedInMove = 0;
        goalPos = transform.position + new Vector3(Random.Range(-maxLengthOfEachMove, maxLengthOfEachMove), 0, Random.Range(-maxLengthOfEachMove, maxLengthOfEachMove));
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
        Vector3 playerPosLeveled = Player.position;
        playerPosLeveled.y = transform.position.y;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (Player.position - transform.position).normalized, out hit, radiusForWanderTemp))
        {
            return hit.collider.gameObject.layer == 9 && !Physics.Raycast(transform.position, (playerPosLeveled - transform.position).normalized, radiusForWanderTemp, ~(1 << 9));
        }
        return false;
    }

    public override Vector2 GetRotation()
    {
        Vector2 posDiffXZ = new Vector2(goalPos.x - transform.position.x, goalPos.z - transform.position.z);
        float angle = (-Mathf.Atan2(posDiffXZ.y, posDiffXZ.x) * Mathf.Rad2Deg) + 180;
        return new Vector2(angle, rotSpeed);
    }
}
