using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBoost : AMove
{
    float duration;
    float horizVel;
    bool jumpPending;
    bool endPending;

    public GroundBoost(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float propCharged) : base(ms, mi, mii)
    {
        duration = movementSettings.HorizBoostMinLengthGroundX + (propCharged * (movementSettings.HorizBoostMaxLengthGroundX - movementSettings.HorizBoostMinLengthGroundX));
        horizVel += movementSettings.HorizBoostMinSpeedIncreaseGroundX + (propCharged * (movementSettings.HorizBoostMaxSpeedIncreaseGroundX - movementSettings.HorizBoostMinSpeedIncreaseGroundX));
        horizVel = Mathf.Clamp(horizVel, movementSettings.HorizBoostMinSpeedX, movementSettings.MaxSpeedAbsolute);
        mii.OnJump.AddListener(() => jumpPending = true);
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitForBoostEnd());
    }

    IEnumerator WaitForBoostEnd()
    {
        yield return new WaitForSeconds(duration);
        endPending = true;
    }

    public override void AdvanceTime()
    {
        // Nothing yet
    }

    public override string AsString()
    {
        return "groundboost";
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return ForwardMovement(horizVel);
    }

    public override IMove GetNextMove()
    {
        if (!mi.TouchingGround())
        {
            return new Fall(mii, mi, movementSettings, ForwardMovement(horizVel), true);
        }
        if (jumpPending)
        {
            return new Jump(mii, mi, movementSettings, movementSettings.GroundBoostRotationSpeed);
        }
        if (endPending)
        {
            return new Run(mii, mi, movementSettings, ForwardMovement(horizVel));
        }
        return this;
    }

    public override RotationInfo GetRotationInfo()
    {
        return new RotationInfo(movementSettings.GroundBoostRotationSpeed, false);
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return true;
    }

    public override bool AdjustToSlope()
    {
        return true;
    }
}
