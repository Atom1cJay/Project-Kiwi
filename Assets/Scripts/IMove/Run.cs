using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Represents movement on the ground while jumping is not occurring.
/// </summary>
public class Run : AMove
{
    float horizVel;
    readonly MovementInputInfo mii;
    readonly MovementInfo mi;
    bool jumpPending;
    bool timeBetweenJumpsBreaksTJ;

    public Run(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        this.mii = mii;
        mii.OnJump.AddListener(() => jumpPending = true);
        this.mi = mi;
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitToBreakTimeBetweenJumps());
    }

    public override void AdvanceTime()
    {
        // Horizontal
        if (mi.currentSpeedHoriz > movementSettings.MaxSpeed)
        {
            horizVel =
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.RunSensitivityX,
                    movementSettings.RunGravityX);
        }
        else
        {
            horizVel =
                InputUtils.SmoothedInput(
                    mi.currentSpeedHoriz,
                    mii.GetHorizontalInput().magnitude * movementSettings.MaxSpeed,
                    movementSettings.RunSensitivityX,
                    movementSettings.RunGravityX);
        }
    }

    // Call with MonobehaviourUtils for a coroutine
    IEnumerator WaitToBreakTimeBetweenJumps()
    {
        yield return new WaitForSeconds(movementSettings.TjMaxTimeBtwnJumps);
        timeBetweenJumpsBreaksTJ = true;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return horizVel;
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override float GetRotationSpeed()
    {
        return movementSettings.GroundRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (mi.currentSpeedHoriz == 0)
        {
            return new Idle(mm, mii, mi, movementSettings);
        }
        if (jumpPending && mi.NextJumpIsTripleJump())
        {
            return new TripleJump(mm, mii, mi, movementSettings);
        }
        if (jumpPending)
        {
            return new Jump(mm, mii, mi, movementSettings);
        }
        if (!mi.TouchingGround())
        {
            return new Fall(mm, mii, mi, movementSettings);
        }
        if (mii.HardTurnInput())
        {
            return new HardTurn(mm, mii, mi, movementSettings);
        }
        // todo make ground boost possible

        return this;
    }

    public override string AsString()
    {
        return "run";
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return mii.GetHorizDissonance() > movementSettings.TjMaxDissonance
            || mii.GetHorizontalInput().magnitude < movementSettings.TjMinHorizInputMagnitude
            || timeBetweenJumpsBreaksTJ;
    }
}
