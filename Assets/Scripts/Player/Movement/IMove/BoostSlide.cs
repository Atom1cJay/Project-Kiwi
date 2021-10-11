using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSlide : AMove
{
    float horizVel;
    bool boostChargePending;

    public BoostSlide(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;
        mii.OnHorizBoostCharge.AddListener(() => boostChargePending = true);
    }

    public override void AdvanceTime()
    {
        Vector2 horizInput = mii.GetHorizontalInput();
        bool fwdInput = horizInput.magnitude > 0 && Mathf.Sin(Mathf.Atan2(horizInput.y, horizInput.x)) > 0;
        // TODO proper charge system
        if (fwdInput)
        {
            horizVel -= movementSettings.BoostSlideSpeedDecRate * Time.deltaTime;
        }
        else
        {
            horizVel -= movementSettings.BoostSlideSpeedDecRateNoInput * Time.deltaTime;
        }
        if (horizVel < 0)
        {
            horizVel = 0;
        }
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return ForwardMovement(horizVel);
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override float GetRotationSpeed()
    {
        return movementSettings.BoostSlideRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (mii.GetInputActions().Player.Jump.ReadValue<float>() > 0)
        {
            return new BoostSlideHop(mii, mi, movementSettings, horizVel);
        }
        if (!mi.TouchingGround())
        {
            return new BoostSlideFall(mii, mi, movementSettings, horizVel);
        }
        if (horizVel <= 0)
        {
            return new Idle(mii, mi, movementSettings);
        }
        if (boostChargePending)
        {
            return new HorizGroundBoostCharge(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        return this;
    }

    public override string AsString()
    {
        return "boostslide";
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
