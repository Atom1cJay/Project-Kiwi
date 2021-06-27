using System;

public class Idle : AMove
{
    MovementInputInfo mii;
    MovementInfo mi;

    public Idle(MovementMaster mm, MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms) : base(mm, ms)
    {
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
            return new Run(mm, mii, mi, movementSettings);
        }
        if (mm.IsJumping() && mm.tripleJumpValid())
        {
            return new TripleJump(mm, mii, mi, movementSettings);
        }
        if (mm.IsJumping())
        {
            return new Jump(mm, mii, mi, movementSettings);
        }
        if (!mm.IsOnGround())
        {
            return new Fall(mm, mii, mi, movementSettings);
        }
        if (mm.IsInHardTurn())
        {
            return new HardTurn(mm, mii, mi, movementSettings);
        }
        // todo make ground boost possible

        return this;
    }

    public override float GetVertSpeedThisFrame()
    {
        return 0;
    }
}
