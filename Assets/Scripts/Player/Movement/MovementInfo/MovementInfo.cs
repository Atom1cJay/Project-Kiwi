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
    private Vector3 prevPosXZ;
    private float effectiveSpeedXZ;
    //private float prevDeltaTime;
    private CharacterController charCont;

    [HideInInspector] public UnityEvent OnCharContTouchSomething;

    private void Awake()
    {
        if (instance != null)
        {
            print("Multiple instances of MovementInfo exist. Use one.");
        }
        instance = this;
        //prevDeltaTime = 0.01f;
        prevPosXZ = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        effectiveSpeedXZ = 0;
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

    private void LateUpdate()
    {
        UpdateEffectiveSpeed();
        UpdateTripleJumpStatus();
    }

    private void UpdateEffectiveSpeed()
    {
        if (Time.timeScale != 0) // To avoid weird bugs
        {
            Vector3 currentXZ = new Vector3(transform.position.x, 0, transform.position.z);
            effectiveSpeedXZ = (currentXZ - prevPosXZ).magnitude / Time.deltaTime;
            prevPosXZ = currentXZ;
            //prevDeltaTime = Time.deltaTime;
        }
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

    /// <summary>
    /// Gives the water detector being used for movement calculations.
    /// </summary>
    public WaterDetector GetWaterDetector()
    {
        return waterDetector;
    }

    public float GetEffectiveSpeed()
    {
        return effectiveSpeedXZ;
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
