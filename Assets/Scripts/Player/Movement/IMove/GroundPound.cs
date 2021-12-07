using System;
using System.Collections;
using UnityEngine;

public class GroundPound : AMove
{
    float timePassed;
    bool suspended = true;
    float vertVel;
    bool divePending;
    bool landingStarted;
    bool landingOver;

    public GroundPound(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(ms, mi, mii)
    {
        mii.OnDiveInput.AddListener(() => divePending = true);
        vertVel = 0;
    }

    public override void AdvanceTime()
    {
        // Pass time
        timePassed += Time.deltaTime;
        // Decide how to handle vertVel based on time passed
        if (suspended && timePassed > movementSettings.GpSuspensionTime)
        {
            vertVel = -movementSettings.GpDownSpeed;
            suspended = false;
        }
        if (!suspended)
        {
            vertVel -= movementSettings.GpDownGravity * Time.deltaTime;
        }
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
        return vertVel;
    }

    public override float GetRotationSpeed()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        // Handle Feedback Moves
        IMove feedbackMove = GetFeedbackMove(Vector2.zero);
        if (feedbackMove != null)
        {
            return feedbackMove;
        }
        // Handle Everything Else
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
