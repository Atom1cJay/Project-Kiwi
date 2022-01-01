using System;
using UnityEngine;

public class HelplessFall : AMove
{
    Vector2 initHorizVector;
    Vector2 horizVector;
    float vertVel;
    //bool swimPending;

    public HelplessFall(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(ms, mi, mii)
    {
        initHorizVector = horizVector;
        this.horizVector = horizVector;
        /*
        if (mi.GetWaterDetector() != null)
        {
            mi.GetWaterDetector().OnHitWater.AddListener(() => swimPending = true);
        }
        */
    }

    public override bool AdjustToSlope()
    {
        return true;
    }

    public override void AdvanceTime()
    {
        horizVector -= (initHorizVector * movementSettings.SlideRecoveryPace) * Time.deltaTime;
        vertVel -= movementSettings.DefaultGravity * Time.deltaTime;
    }

    public override string AsString()
    {
        return "helplessfall";
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return horizVector;
    }

    public override IMove GetNextMove()
    {
        /*
        if (swimPending)
        {
            return new Swim(mii, mi, movementSettings, horizVector);
        }
        */
        if (PlayerSlopeHandler.ShouldSlide)
        {
            return new Slide(mii, mi, movementSettings, horizVector);
        }
        if (!mi.TouchingGround())
        {
            return new Fall(mii, mi, movementSettings, horizVector, false);
        }
        if (mi.TouchingGround())
        {
            return new SlideRecovery(mii, mi, movementSettings, horizVector);
        }
        return this;
    }

    public override RotationInfo GetRotationInfo()
    {
        return new RotationInfo(0, false);
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
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
