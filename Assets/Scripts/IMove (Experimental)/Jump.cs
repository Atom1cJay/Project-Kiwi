using System;
using UnityEngine;
using System.Collections;

public class Jump : AMove
{
    float gravity;
    float vertVel;

    public Jump(HorizontalMovement hm, VerticalMovement vm, MovementMaster mm) : base(hm, vm, mm)
    {
        gravity = vm.initGravity;
        vertVel = vm.initJumpVel;
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
        else if (hm.currentSpeed > hm.defaultMaxSpeed)
        {
            toReturn =
                InputUtils.SmoothedInput(
                    hm.currentSpeed,
                    hm.getHorizontalInput().magnitude * hm.defaultMaxSpeed,
                    hm.sensitivity,
                    hm.overTopSpeedAirGravity);
        }
        else
        {
            toReturn =
                InputUtils.SmoothedInput(
                    hm.currentSpeed,
                    hm.getHorizontalInput().magnitude * hm.defaultMaxSpeed,
                    hm.airSensitivity,
                    hm.airGravity);
        }

        return toReturn;
    }

    public override float GetVertSpeedThisFrame()
    {
        // Decide Gravity
        if (mm.JumpInputCancelled())
            gravity += vm.gravityIncRateAtCancel * Time.fixedDeltaTime;
        else
            gravity += vm.gravityIncRate * Time.fixedDeltaTime;

        if (gravity > vm.maxGravity && !mm.JumpInputCancelled())
            gravity = vm.maxGravity;
        else if (gravity > vm.maxGravityAtCancel && mm.JumpInputCancelled())
            gravity = vm.maxGravityAtCancel;

        // Effect Gravity
        vertVel -= gravity * Time.fixedDeltaTime;
        return vertVel;
    }

    private void OnJumpCanceled()
    {
        // todo add a point of no return?
        if (vertVel > 0)
        {
            vertVel *= vm.velocityMultiplierAtCancel;
        }
    }

    public override IMove GetNextMove()
    {
        throw new NotImplementedException();
    }
}
   
