using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBeeChase : AAIState
{
    [SerializeField] float timeForState;
    [SerializeField] float speed;
    Vector3 goalPos;
    float timePassed;

    public override void AdvanceTime()
    {
        timePassed += Time.deltaTime;
    }

    public override string GetAnimationID()
    {
        return "chase";
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
        goalPos = Player.position;
        timePassed = 0;
    }

    public override bool DestroysEnemy()
    {
        return false;
    }

    public override bool ShouldBeginState()
    {
        Vector3 playerPosLeveled = Player.position;
        playerPosLeveled.y = transform.position.y;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (Player.position - transform.position).normalized, out hit, (Player.position - transform.position).magnitude))
        {
            return hit.collider.gameObject.layer == 9 && !Physics.Raycast(transform.position, (playerPosLeveled - transform.position).normalized, (Player.position - transform.position).magnitude, ~(1 << 9));
        }
        return false;
    }

    public override bool ShouldFinishState()
    {
        return timePassed > timeForState || transform.position == goalPos;
    }

    public override Vector2 GetRotation()
    {
        return new Vector2(0, 0);
    }
}
