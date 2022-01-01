using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : AMove
{
    public Death(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(ms, mi, mii)
    {
        // Nothing
    }

    public override void AdvanceTime()
    {
        // Nothing
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return Vector2.zero;
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override RotationInfo GetRotationInfo()
    {
        return new RotationInfo(0, false);
    }

    public override bool AdjustToSlope()
    {
        return false;
    }

    public override string AsString()
    {
        return "death";
    }

    public override IMove GetNextMove()
    {
        return this; // No transition from death
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
