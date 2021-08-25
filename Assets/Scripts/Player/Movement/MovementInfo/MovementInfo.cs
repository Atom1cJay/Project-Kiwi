using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MoveExecuter))]
public class MovementInfo : MonoBehaviour
{
    public static MovementInfo instance;
    [SerializeField] CollisionDetector groundDetector;
    [SerializeField] CollisionDetector antiBoostDetector;
    [SerializeField] WaterDetector waterDetector;
    private int tjJumpCount;
    IMoveImmutable storedMove; // The move from the last frame
    MoveExecuter me;
    private Vector2 effectiveSpeedXZ;
    private Vector2 effectiveSpeedXZReturnable;
    private CharacterController charCont;

    // Testing
    private List<Vector2> lastFiveMoves = new List<Vector2>(
        new []{ Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero });
    private Vector2 lastFiveMovesAccum = Vector2.zero;
    private Vector2 prevPosXZ;

    [HideInInspector] public UnityEvent OnCharContTouchSomething;

    private void Awake()
    {
        if (instance != null)
        {
            print("Multiple instances of MovementInfo exist. Use one.");
        }
        instance = this;
        prevPosXZ = new Vector2(transform.position.x, transform.position.z);
        effectiveSpeedXZ = Vector2.zero;
        effectiveSpeedXZReturnable = Vector2.zero;
        me = GetComponent<MoveExecuter>();
        charCont = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Determines whether the player is currently touching the ground.
    /// </summary>
    public bool TouchingGround()
    {
        return groundDetector.Colliding();
    }

    /// <summary>
    /// Determines whether the player is currently touching the ground.
    /// </summary>
    public bool InAntiBoostZone()
    {
        return antiBoostDetector.Colliding();
    }

    private void FixedUpdate()
    {
        /*
        fixedMoveThisIteration = new Vector2(transform.position.x, transform.position.z) - lastXZFixed - lateMoveAccum;
        fixedMoveAccum += fixedMoveThisIteration;
        lastXZFixed = new Vector2(transform.position.x, transform.position.z);
        lateMoveAccum = Vector2.zero;
        print("Fixed Speed This Iteration: " + fixedMoveThisIteration / Time.fixedDeltaTime);*/
        //UpdateEffectiveSpeedFixed();
    }

    private void LateUpdate()
    {
        Vector2 curXZ = new Vector2(transform.position.x, transform.position.z);
        lastFiveMoves.Add((curXZ - prevPosXZ) / Time.deltaTime);
        lastFiveMovesAccum += lastFiveMoves[5];
        lastFiveMovesAccum -= lastFiveMoves[0];
        lastFiveMoves.RemoveAt(0);
        prevPosXZ = curXZ;
        print(lastFiveMovesAccum / 5);

        UpdateTripleJumpStatus();
    }

    /*
    private void UpdateEffectiveSpeedFixed()
    {
        if (Time.timeScale != 0) // To avoid weird bugs
        {
            Vector2 currentXZ = new Vector2(transform.position.x, transform.position.z);
            effectiveSpeedXZ += (currentXZ - prevPosXZ);
            effectiveSpeedXZReturnable = effectiveSpeedXZ / Time.deltaTime;
            print(effectiveSpeedXZReturnable);
            prevPosXZ = currentXZ;
        }
    }

    private void UpdateEffectiveSpeedLate()
    {
        if (Time.timeScale != 0) // To avoid weird bugs
        {
            Vector2 currentXZ = new Vector2(transform.position.x, transform.position.z);
            effectiveSpeedXZ += (currentXZ - prevPosXZ);
            effectiveSpeedXZ = Vector2.zero;
            prevPosXZ = currentXZ;
        }
    }
    */

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

    /// <summary>
    /// Gives the water detector being used for movement calculations.
    /// </summary>
    public WaterDetector GetWaterDetector()
    {
        return waterDetector;
    }

    public Vector2 GetEffectiveSpeed()
    {
        return lastFiveMoves[4];
    }

    public Vector2 GetAvgSpeed5() // Average speed of the past 5 lateupdates
    {
        return lastFiveMovesAccum / 5;
    }

    public Transform GetPlayerTransform()
    {
        return transform;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.controller == charCont)
        {
            OnCharContTouchSomething.Invoke();
        }
    }
}
