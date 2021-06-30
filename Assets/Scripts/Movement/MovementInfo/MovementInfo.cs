using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveExecuter))]
public class MovementInfo : MonoBehaviour
{
    [SerializeField] CollisionDetector groundDetector;
    private int tjJumpCount;
    IMoveImmutable storedMove; // The move from the last frame
    MoveExecuter me;

    private void Awake()
    {
        me = GetComponent<MoveExecuter>();
    }

    /// <summary>
    /// Determines whether the player is currently touching the ground.
    /// </summary>
    public bool TouchingGround()
    {
        return groundDetector.Colliding();
    }

    private void Update()
    {
        UpdateTripleJumpStatus();
    }

    /// <summary>
    /// Decides what to do with the current triple jump count this frame
    /// (whether to keep it as is, or reset it to 0, or increment it).
    /// </summary>
    private void UpdateTripleJumpStatus()
    {
        IMoveImmutable curMove = me.GetCurrentMove();
        if (curMove.TJshouldBreak())
        {
            tjJumpCount = 0;
        }
        if (curMove != storedMove)
        {
            tjJumpCount += curMove.IncrementsTJcounter() ? 1 : 0;
            storedMove = me.GetCurrentMove();
        }
    }

    /// <summary>
    /// Determines whether, if a jump was done right at this frame, it should
    /// be a triple jump.
    /// </summary>
    public bool NextJumpIsTripleJump()
    {
        UpdateTripleJumpStatus();
        return tjJumpCount == 2;
    }

    /// <summary>
    /// Determines whether, if a jump was done right at this frame, it should
    /// be a double jump.
    /// </summary>
    public bool NextJumpIsDoubleJump()
    {
        UpdateTripleJumpStatus();
        return tjJumpCount == 1;
    }

    /// <summary>
    /// Gives the ground detector being used for movement calculations.
    /// </summary>
    public CollisionDetector GetGroundDetector()
    {
        return groundDetector;
    }
}
