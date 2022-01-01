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
        return true;
    }

    public override bool ShouldFinishState()
    {
        return timePassed > timeForState;
    }

    public override Vector2 GetRotation()
    {
        return new Vector2(0, 0);
    }
}
