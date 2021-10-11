using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSlideHop : AMove
{
    bool landableTimerPassed;
    float vertVel;
    float horizVel;

    public BoostSlideHop(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        vertVel = movementSettings.BoostHopInitVelY;
        if (horizVel >= movementSettings.BoostHopXVelForMoving)
        {
            this.horizVel = movementSettings.BoostHopInitVelXMoving;
        }
        else
        {
            this.horizVel = movementSettings.BoostHopInitVelXStopped;
        }
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", RunLandableTimer());
    }

    IEnumerator RunLandableTimer()
    {
        yield return new WaitForSeconds(0.1f);
        landableTimerPassed = true;
    }

    public override void AdvanceTime()
    {
        vertVel -= movementSettings.BoostHopGravity * Time.deltaTime;
        /*
        horizVel -= movementSettings.BoostSlideSpeedDecRate * Time.deltaTime;
        if (horizVel < 0)
        {
            horizVel = 0;
        }
        */
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return ForwardMovement(horizVel);
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override float GetRotationSpeed()
    {
        return movementSettings.BoostSlideRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (mi.TouchingGround() && landableTimerPassed)
        {
            return new Run(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        return this;
    }

    public override string AsString()
    {
        return "boostslidehop";
    }

    public override bool AdjustToSlope()
    {
        return true;
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return true;
    }
}
