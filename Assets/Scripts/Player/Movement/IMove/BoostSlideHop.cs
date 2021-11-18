using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSlideHop : AMove
{
    bool landableTimerPassed;
    float vertVel;
    float horizVel;
    bool swimPending;
    private bool receivedBasicHit;
    private Vector3 basicHitNormal;

    public BoostSlideHop(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        vertVel = movementSettings.BoostHopInitVelY;
        this.horizVel = horizVel * movementSettings.BoostHopInitVelXMultiplier;
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", RunLandableTimer());
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
        mi.ph.onBasicHit.AddListener((Vector3 basicHitNormal) => { receivedBasicHit = true; this.basicHitNormal = basicHitNormal; });
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
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (receivedBasicHit)
        {
            return new Knockback(mii, mi, movementSettings, basicHitNormal, ForwardMovement(horizVel));
        }
        if (PlayerSlopeHandler.ShouldSlide)
        {
            return new Slide(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        if (mi.TouchingGround() && landableTimerPassed)
        {
            return new Run(mii, mi, movementSettings, ForwardMovement(horizVel), FromStatus.FromAir);
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
