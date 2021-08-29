using System;
using System.Collections;
using UnityEngine;

public class GroundPound : AMove
{
    float timePassed;
    bool divePending;
    bool landingStarted;
    bool landingOver;
    bool swimPending;

    public GroundPound(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(ms, mi, mii)
    {
        mii.OnDiveInput.AddListener(() => divePending = true);
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
    }

    public override void AdvanceTime()
    {
        timePassed += Time.deltaTime;
        if (mi.TouchingGround() && !landingStarted)
        {
            // Check if the ground-pounded object has a specific pound event
            Poundable potentialPoundScript = mi.GetGroundDetector().CollidingWith().GetComponent<Poundable>();
            if (potentialPoundScript != null)
            {
                potentialPoundScript.BroadcastPoundEvent();
            }
            // Initiate the landing portion of the pound
            landingStarted = true;
            MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitForLandingEnd());
        }
    }

    IEnumerator WaitForLandingEnd()
    {
        yield return new WaitForSeconds(movementSettings.GpLandTime);
        landingOver = true;
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return Vector2.zero;
    }

    public override float GetVertSpeedThisFrame()
    {
        if (timePassed < movementSettings.GpSuspensionTime)
        {
            return 0;
        }
        return -movementSettings.GpDownSpeed;
    }

    public override float GetRotationSpeed()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, Vector2.zero);
        }
        if (divePending && !landingStarted)
        {
            return new Dive(mii, mi, movementSettings);
        }
        if (landingOver)
        {
            return new Idle(mii, mi, movementSettings);
        }
        return this;
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return true;
    }

    public override bool AdjustToSlope()
    {
        return false;
    }

    public override string AsString()
    {
        return "groundpound";
    }

    public override Attack GetAttack()
    {
        return movementSettings.GroundPoundAttack;
    }
}
