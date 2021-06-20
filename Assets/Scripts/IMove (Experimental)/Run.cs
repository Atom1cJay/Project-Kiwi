using System;
using UnityEngine;

/// <summary>
/// Represents movement on the ground while jumping is not occurring.
/// </summary>
public class Run : AMove
{
    public Run(HorizontalMovement hm, VerticalMovement vm, MovementMaster mm) : base(hm, vm, mm) { }

    public override float GetHorizSpeedThisFrame()
    {
        if (hm.currentSpeed > hm.defaultMaxSpeed)
        {
            return
                InputUtils.SmoothedInput(
                    hm.currentSpeed,
                    hm.getHorizontalInput().magnitude * hm.defaultMaxSpeed,
                    hm.sensitivity,
                    hm.gravity);
        }
        else
        {
            return
                InputUtils.SmoothedInput(
                    hm.currentSpeed,
                    hm.getHorizontalInput().magnitude * hm.defaultMaxSpeed,
                    hm.sensitivity,
                    hm.gravity);
        }
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return "run";
    }
}
