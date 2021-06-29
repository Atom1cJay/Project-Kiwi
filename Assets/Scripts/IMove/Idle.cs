using System;

public class Idle : AMove
{
    readonly MovementInputInfo mii;
    readonly MovementInfo mi;
    bool jumpPending;

    public Idle(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        this.mii = mii;
        mii.OnJump.AddListener(() => jumpPending = true);
        this.mi = mi;
    }

    public override void AdvanceTime()
    {
        // Nothing changes over time
    }

    public override float GetHorizSpeedThisFrame()
    {
        return 0;
    }

    public override float GetRotationSpeed()
    {
        return movementSettings.GroundRotationSpeed;
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        if (mii.GetHorizontalInput().magnitude != 0)
        {
            return new Run(mm, mii, mi, movementSettings);
        }
        if (jumpPending)
        {
            return new Jump(mm, mii, mi, movementSettings);
        }
        if (!mi.TouchingGround())
        {
            return new Fall(mm, mii, mi, movementSettings);
        }
        // todo make ground boost possible

        return this;
    }

    public override string AsString()
    {
        return "idle";
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
