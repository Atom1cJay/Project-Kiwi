using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSlideHop : AMove
{
    bool landableTimerPassed;
    float vertVel;
    float horizVel;
    bool objectHitPending;

    public BoostSlideHop(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        vertVel = movementSettings.BoostHopInitVelY;
        this.horizVel = horizVel * movementSettings.BoostHopInitVelXMultiplier;
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", RunLandableTimer());
        mi.OnCharContTouchSomething.AddListener(() => objectHitPending = true);
    }

    IEnumerator RunLandableTimer()
    {
        yield return new WaitForSeconds(0.1f);
        landableTimerPassed = true;
    }

    public override void AdvanceTime()
    {
        vertVel -= movementSettings.BoostHopGravity * Time.deltaTime;
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
        return movementSettings.BoostSlideHopRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        // Handle Feedback Moves
        IMove feedbackMove = GetFeedbackMove(ForwardMovement(horizVel));
        if (feedbackMove != null)
        {
            return feedbackMove;
        }
        // Handle Everything Else
        if (PlayerSlopeHandler.ShouldSlide)
        {
            return new Slide(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (mi.TouchingGround() && landableTimerPassed)
        {
            return new Run(mii, mi, movementSettings, ForwardMovement(horizVel), FromStatus.FromAir);
        }
        if (objectHitPending)
        {
            Vector3 kbVector = new Vector3(-ForwardMovement(horizVel).x, 0, -ForwardMovement(horizVel).y);
            return new Knockback(mii, mi, movementSettings, kbVector, ForwardMovement(horizVel));
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
