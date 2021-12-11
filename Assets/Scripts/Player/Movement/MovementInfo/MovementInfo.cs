using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MoveExecuter))]
[RequireComponent(typeof(PlayerHealth))]
public class MovementInfo : MonoBehaviour
{
    public static MovementInfo instance;
    [SerializeField] CollisionDetector groundDetector;
    [SerializeField] CollisionDetector waterDetector;
    [SerializeField] CollisionDetector antiBoostDetector;
    public PlayerHealth ph { get; private set; }
    private int tjJumpCount;
    IMoveImmutable storedMove; // The move from the last frame
    MoveExecuter me;
    private CharacterController charCont;

    // Testing
    private List<Vector2> lastFiveMoves = new List<Vector2>(
        new []{ Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero });
    private Vector2 prevPosXZ;

    [HideInInspector] public UnityEvent OnCharContTouchSomething;
    [HideInInspector] public UnityEvent onJumpAttackFeedbackReceived = new UnityEvent();

    private void Awake()
    {
        if (instance != null)
        {
            print("Multiple instances of MovementInfo exist. Use one.");
        }
        instance = this;
        prevPosXZ = new Vector2(transform.position.x, transform.position.z);
        me = GetComponent<MoveExecuter>();
        charCont = GetComponent<CharacterController>();
        ph = GetComponent<PlayerHealth>();
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
        if (Time.timeScale > 0 && Time.deltaTime > 0)
        {
            UpdateEffectiveSpeed();
        }
        UpdateTripleJumpStatus();
    }

    private void UpdateEffectiveSpeed()
    {
        Vector2 curXZ = new Vector2(transform.position.x, transform.position.z);
        lastFiveMoves.Add((curXZ - prevPosXZ) / Time.deltaTime);
        lastFiveMoves.RemoveAt(0);
        prevPosXZ = curXZ;
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

    // Is the water detector colliding with anything?
    public bool TouchingWater()
    {
        return waterDetector.Colliding();
    }

    public Vector2 GetEffectiveSpeed()
    {
        return lastFiveMoves[4];
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

    // The player has just hit an enemy using a jump attack. The player should jump
    // in response. Fire off the proper event to do so.
    public void TakeJumpAttackFeedback()
    {
        print("Jump attack feedback");
        onJumpAttackFeedbackReceived.Invoke();
    }
}
