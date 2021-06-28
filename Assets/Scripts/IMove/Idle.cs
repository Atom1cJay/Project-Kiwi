using System;

public class Idle : AMove
{
    MovementInputInfo mii;
    MovementInfo mi;
    bool jumpPending;

    public Idle(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
        this.mii = mii;
        mii.OnJump.AddListener(() => jumpPending = true);
        this.mi = mi;
    }

    public override float GetHorizSpeedThisFrame()
    {
        return 0;
    }

    public override float GetRotationThisFrame()
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
        if (jumpPending && mm.tripleJumpValid())
        {
            return new TripleJump(mm, mii, mi, movementSettings);
        }
        if (jumpPending)
        {
            return new Jump(mm, mii, mi, movementSettings);
        }
        if (!mm.IsOnGround())
        {
            return new Fall(mm, mii, mi, movementSettings);
        }
        /*
        if (mm.IsInHardTurn())
        {
            return new HardTurn(mm, mii, mi, movementSettings);
        }
        */
        // todo make ground boost possible

        return this;
    }

    public override string asString()
    {
        return "idle";
    }
}
