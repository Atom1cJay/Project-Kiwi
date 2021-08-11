using System;
using System.Collections;
using UnityEngine;

public class SlideRecovery : AMove
{
    Vector2 initHorizVector;
    Vector2 horizVector;
    bool recovered;

    public SlideRecovery(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(ms, mi, mii)
    {
        initHorizVector = horizVector;
        this.horizVector = horizVector;
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitForRecoveryTimeEnd());
    }

    IEnumerator WaitForRecoveryTimeEnd()
    {
        yield return new WaitForSeconds(1f / movementSettings.SlideRecoveryPace);
        recovered = true;
    }

    public override bool AdjustToSlope()
    {
        return true;
    }

    public override void AdvanceTime()
    {
        horizVector -= (initHorizVector * movementSettings.SlideRecoveryPace) * Time.deltaTime;
    }

    public override string AsString()
    {
        return "sliderecovery";
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return horizVector;
    }

    public override IMove GetNextMove()
    {
        if (recovered)
        {
            return new Idle(mii, mi, movementSettings);
        }
        if (PlayerSlopeHandler.BeyondMaxAngle && mi.TouchingGround())
        {
            return new Slide(mii, mi, movementSettings, horizVector);
        }
        if (!mi.TouchingGround())
        {
            return new Fall(mii, mi, movementSettings, horizVector, false);
        }
        return this;
    }

    public override float GetRotationSpeed()
    {
        return 0;
    }

    public override float GetVertSpeedThisFrame()
    {
        return -0.9f;
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
