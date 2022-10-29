using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MoveExecuter))]
[RequireComponent(typeof(PlayerHealth))]
public class MovementInfo : MonoBehaviour
{
    public static MovementInfo instance; // There can be only one

    // A bunch of collision detectors
    [SerializeField] CollisionDetector groundDetector;
    [SerializeField] CollisionDetector bonkDetector;
    [SerializeField] CollisionDetector waterDetector;
    [SerializeField] CollisionDetector antiBoostDetector;
    [SerializeField] CollisionDetector waterBoopDetector;

    // Triple Jump Tracking
    private int tjJumpCount;
    IMoveImmutable storedMove; // The move from the last frame
    MoveExecuter me;

    // Other
    public PlayerHealth ph { get; private set; }
    Vector2 effectiveSpeed;

    [HideInInspector] public UnityEvent onJumpAttackFeedbackReceived = new UnityEvent();

    private void Awake()
    {
        if (instance != null)
        {
            print("Multiple instances of MovementInfo exist. Use one.");
        }
        instance = this;
        me = GetComponent<MoveExecuter>();
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
    /// Determines whether the player's bonk detector is touching anything.
    /// </summary>
    public bool BonkDetectorTouching()
    {
        return bonkDetector.Colliding();
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
        UpdateTripleJumpStatus();
    }

    /// <summary>
    /// Set the Effective speed to the current value. This function
    /// should probably only be called by MoveExecuter.
    /// </summary>
    public void UpdateEffectiveSpeed(Vector2 es)
    {
        effectiveSpeed = es;
    }

    /// <summary>
    /// Decides what to do with the current triple jump count this frame
    /// (whether to keep it as is, or reset it to 0, or increment it).
    /// </summary>
    private void UpdateTripleJumpStatus()
    {
        IMoveImmutable curMove = MoveExecuter.instance.GetCurrentMove();
        if (curMove.TJshouldBreak())
        {
            tjJumpCount = 0;
        }
        if (curMove != storedMove)
        {
            tjJumpCount += curMove.IncrementsTJcounter() ? 1 : 0;
            storedMove = MoveExecuter.instance.GetCurrentMove();
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
    /// Gives the bonk detector being used for movement calculations.
    /// </summary>
    public CollisionDetector GetBonkDetector()
    {
        return bonkDetector;
    }

    // Is the water detector (i.e. the one that should be used for swimming)
    // colliding with anything?
    public bool TouchingWater()
    {
        return waterDetector.Colliding();
    }

    /// <summary>
    /// Is the player touching the water (visually - NOT the detector that is
    /// checked for swimming)
    /// </summary>
    public bool BoopingWater()
    {
        return waterBoopDetector.Colliding();
    }

    public Vector2 GetEffectiveSpeed()
    {
        return effectiveSpeed;
    }

    public Transform GetPlayerTransform()
    {
        return transform;
    }

    // The player has just hit an enemy using a jump attack. The player should jump
    // in response. Fire off the proper event to do so.
    public void TakeJumpAttackFeedback()
    {
        print("Jump attack feedback");
        onJumpAttackFeedbackReceived.Invoke();
    }
}
