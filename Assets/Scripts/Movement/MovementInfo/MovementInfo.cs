using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MoveExecuter))]
public class MovementInfo : MonoBehaviour
{
    [SerializeField] CollisionDetector groundDetector;
    private int tjJumpCount;
    IMoveImmutable storedMove; // The move from the last frame
    MoveExecuter me;
    private Vector3 prevPosXZ;
    private float effectiveSpeedXZ;
    private float prevDeltaTime;

    private void Awake()
    {
        prevDeltaTime = 0.01f;
        prevPosXZ = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        effectiveSpeedXZ = 0;
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
        UpdateEffectiveSpeed();
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

    private void UpdateEffectiveSpeed()
    {
        Vector3 currentXZ = new Vector3(transform.position.x, 0, transform.position.z);
        effectiveSpeedXZ = (currentXZ - prevPosXZ).magnitude / prevDeltaTime;
        prevPosXZ = currentXZ;
        prevDeltaTime = Time.deltaTime;
    }

    public float GetEffectiveSpeed()
    {
        return effectiveSpeedXZ;
    }

    public Transform GetPlayerTransform()
    {
        return transform;
    }
}