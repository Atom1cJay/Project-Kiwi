using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSlideHop : AMove
{
    bool landableTimerPassed;
    float vertVel;
    float horizVel;
    bool boostChargePending;

    public BoostSlideHop(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        vertVel = movementSettings.BoostHopInitVelY;
        this.horizVel = mi.GetEffectiveSpeed().magnitude;
        //this.horizVel = horizVel * movementSettings.BoostHopInitVelXMultiplier;
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", RunLandableTimer());
        mii.OnHorizBoostCharge.AddListener(() => boostChargePending = true);
        //mi.OnCharContTouchSomething.AddListener(() => objectHitPending = true);
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

    public override RotationInfo GetRotationInfo()
    {
        return new RotationInfo(movementSettings.BoostSlideHopRotationSpeed, false);
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
        if (mi.BonkDetectorTouching())
        {
            //Vector3 kbVector = new Vector3(-ForwardMovement(horizVel).x, 0, -ForwardMovement(horizVel).y);
            return new Knockback(mii, mi, movementSettings, Vector3.zero, ForwardMovement(horizVel));
        }
        if (boostChargePending)
        {
            return new HorizAirBoostCharge(mii, mi, movementSettings, vertVel, ForwardMovement(horizVel));
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

    public override Attack[] GetAttack()
    {
        return new Attack[] { movementSettings.JumpAttack };
    }
}
