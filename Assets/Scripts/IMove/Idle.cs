using System;

public class Idle : AMove
{
    MovementSettings ms;
    MovementInputInfo mii;
    MovementInfo mi;

    public Idle(MovementMaster mm, MovementSettings ms, MovementInputInfo mii, MovementInfo mi) : base(mm)
    {
        this.ms = ms;
        this.mii = mii;
        this.mi = mi;
    }

    public override string asString()
    {
        return "idle";
    }

    public override float GetHorizSpeedThisFrame()
    {
        return 0;
    }

    public override IMove GetNextMove()
    {
        if (mm.GetHorizontalInput().magnitude != 0)
        {
            return new Run(mm, ms, mii, mi);
        }
        if (mm.IsJumping() && mm.tripleJumpValid())
        {
            return new TripleJump(mm, ms, mii, mi);
        }
        if (mm.IsJumping())
        {
            return new Jump(mm, ms, mii, mi);
        }
        if (!mm.IsOnGround())
        {
            return new Fall(mm, ms, mii, mi);
        }
        if (mm.IsInHardTurn())
        {
            return new HardTurn(mm, ms, mii, mi);
        }
        // todo make ground boost possible

        return this;
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }
}
