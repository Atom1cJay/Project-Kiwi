using System;
using UnityEngine;
using System.Collections;

public class TripleJump : AMove
{
    float gravity;
    float vertVel;

    public TripleJump(HorizontalMovement hm, VerticalMovement vm, MovementMaster mm) : base(hm, vm, mm)
    {
        gravity = vm.tjInitGravity;
        vertVel = vm.tjInitJumpVel;
        mm.mm_OnJumpCanceled.AddListener(OnJumpCanceled);
    }

    public override float GetHorizSpeedThisFrame()
    {
        float toReturn;

        if (hm.isAirReversing())
        {
            toReturn =
                InputUtils.SmoothedInput(
                    hm.currentSpeed,
                    -hm.getHorizontalInput().magnitude * hm.defaultMaxSpeed,
                    hm.airReverseSensitivity,
                    hm.airReverseGravity);
            if (toReturn < 0) toReturn = 0;
        }
        else
        {
            toReturn =
                InputUtils.SmoothedInput(
                    hm.currentSpeed,
                    hm.getHorizontalInput().magnitude * hm.defaultMaxSpeed,
                    hm.tjAirSensitivity,
                    hm.tjAirGravity);
        }

        return toReturn;
    }

    public override float GetVertSpeedThisFrame()
    {
        // Decide Gravity
        if (mm.JumpInputCancelled())
            gravity += vm.tjGravityIncRateAtCancel * Time.fixedDeltaTime;
        else
            gravity += vm.tjGravityIncRate * Time.fixedDeltaTime;

        if (gravity > vm.tjMaxGravity && !mm.JumpInputCancelled())
            gravity = vm.tjMaxGravity;
        else if (gravity > vm.tjMaxGravityAtCancel && mm.JumpInputCancelled())
            gravity = vm.tjMaxGravityAtCancel;

        // Effect Gravity
        vertVel -= gravity * Time.fixedDeltaTime;
        return vertVel;
    }

    /// <summary>
    /// Marks a jump as cancelled (meaning the arc should begin to decrease)
    /// if we are in the proper part of the jump.
    /// </summary>
    private void OnJumpCanceled()
    {
        // TODO point of no return?
        if (vertVel > 0)
        {
            vertVel *= vm.tjVelocityMultiplierAtCancel;
        }
    }

    public override IMove GetNextMove()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return "triplejump";
    }
}
