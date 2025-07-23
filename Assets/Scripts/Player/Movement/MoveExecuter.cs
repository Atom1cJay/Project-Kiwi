using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The main control center for the player.
/// Decides how the player and camera should move, depending on the currently
/// active IMove.
/// </summary>
[RequireComponent(typeof(MovementInfo))]
[RequireComponent(typeof(MovementInputInfo))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(RotationMovement))]
[RequireComponent(typeof(BumperHandler))]
public class MoveExecuter : MonoBehaviour
{
    IMove moveLastFrame = null;
    static IMove moveThisFrame;
    CharacterController charCont;
    MovementInfo mi;
    MovementInputInfo mii;
    MovementSettingsSO movementSettings;
    RotationMovement rotator;
    BumperHandler bh;
    [SerializeField] CameraControl cameraControl;
    [SerializeField] CameraTarget camTarget;
    [SerializeField] StickToGround shadowCaster;
    [SerializeField] float barrierRadius;
    Ridable ridable = null;

    Vector3 additionalVelocityToAdd = Vector3.zero;

    static MoveExecuter _instance;
    public static MoveExecuter instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Tried to get MoveExecuter instance before it is set in Awake()");
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }


    public delegate void OnMoveChangedDelegate(IMoveImmutable oldMove, IMoveImmutable newMove);

    public event OnMoveChangedDelegate OnMoveChanged;

    private void Awake()
    {
        instance = this;
        OnMoveChanged = null;
        charCont = GetComponent<CharacterController>();
        mi = GetComponent<MovementInfo>();
        mii = GetComponent<MovementInputInfo>();
        rotator = GetComponent<RotationMovement>();
        bh = GetComponent<BumperHandler>();
        Physics.autoSimulation = false;
    }

    private void Start()
    {
        movementSettings = MovementSettingsSO.Instance;
        moveThisFrame = new Fall(mii, mi, movementSettings, Vector2.zero, false);
        //mii.OnRespawnToCheckpointInput.AddListener(() => RespawnPlayerToCheckpoint());
    }

    private void Update()
    {
        if (ridable == null) // If not null, the moving platform's TransformChange event will call it instead
        {
            Move();
        }
    }

    /// <summary>
    /// Handle all forms of movement, and resulting events, for a particular frame.
    /// </summary>
    private void Move()
    {
        HandleBasicMovement();
        HandleMovementChangeEvent();
        UpdateMovingPlatformStatus();
    }

    /// <summary>
    /// Update whether the player is on a moving platform (Ridable) or not.
    /// </summary>
    private void UpdateMovingPlatformStatus()
    {
        if (ridable == null && mi.TouchingGround() && mi.GetGroundDetector().CollidingWith().CompareTag("Moving Platform"))
        {
            ridable = mi.GetGroundDetector().CollidingWith().GetComponentInParent<Ridable>();
            ridable.Register();
            ridable.onTransformChange.AddListener(() => Move());
        }
        if (!moveThisFrame.AdjustToSlope() || (!mi.TouchingGround() && !PlayerSlopeHandler.GroundInProximity) || (mi.TouchingGround() && !mi.GetGroundDetector().CollidingWith().CompareTag("Moving Platform")))
        {
            if (ridable != null)
            {
                ridable.Deregister();
                ridable.onTransformChange.RemoveAllListeners(); // todo bad?
                ridable = null;
            }
        }
    }

    /// <summary>
    /// Add artificial velocity to the player, otherwise preserving the
    /// current move state.
    /// </summary>
    /// <param name="additionalVelocity">The artificial velocity to add</param>
    public void AddAdditionalVelocityThisFrame(Vector3 additionalVelocity)
    {
        additionalVelocityToAdd = additionalVelocity;
    }

    /// <summary>
    /// Applies horizontal and vertical movement for the player,
    /// depending on the currently active IMove. Calls on the camera to adapt
    /// if necessary.
    /// Also moves according to the current moving platform, if there is one.
    /// </summary>
    private void HandleBasicMovement()
    {
        if (Time.timeScale > 0 && Time.deltaTime > 0) // Check for the sake of avoiding weird errors
        {
            Vector2 origPosXZ = new Vector2(transform.position.x, transform.position.z);

            HandleMovingPlatformMovement();
            HandleAdditionalVelocity();

            // Handle bumper physics to ensure stable collision
            bh.HandleBumperMoved();
            Physics.Simulate(Time.deltaTime);
            bh.HandleBumperMoved();
            charCont.Move(Vector3.left * barrierRadius);
            charCont.Move(Vector3.back * barrierRadius);
            charCont.Move(Vector3.right * barrierRadius);
            charCont.Move(Vector3.forward * barrierRadius);

            // Normal IMove movement
            moveThisFrame.AdvanceTime();
            cameraControl.HandleManualControl();
            rotator.DetermineRotation();
            Vector2 horizMovement = moveThisFrame.GetHorizSpeedThisFrame();
            Vector3 dir = DirectionOfMovement(horizMovement);
            Vector3 horizMovementAdjusted = dir * horizMovement.magnitude * Time.deltaTime;
            Vector3 vertMovement = Vector3.up * moveThisFrame.GetVertSpeedThisFrame() * Time.deltaTime;
            Vector3 mvmtTotal = horizMovementAdjusted + vertMovement;
            charCont.Move(mvmtTotal);
            mi.UpdateEffectiveSpeed((new Vector2(transform.position.x, transform.position.z) - origPosXZ) / Time.deltaTime);

            bh.HandleBumperMoved();
            camTarget.Adjust();
            shadowCaster.UpdatePosition(charCont.transform.position);
            IMove next = moveThisFrame.GetNextMove();
            moveThisFrame = next;
        }
    }

    /// <summary>
    /// Apply the extra movement associated with a moving platform, for this frame.
    /// </summary>
    private void HandleMovingPlatformMovement()
    {
        if (ridable != null)
        {
            Vector3 extraMovement = ridable.TranslationThisFrame();
            transform.Translate(extraMovement, Space.World);
            Vector3 extraMovementFromRot = ridable.PlayerPosChangeFromRotThisFrame(new Vector3(transform.position.x, charCont.bounds.min.y, transform.position.z));
            transform.Translate(extraMovementFromRot, Space.World);
            rotator.RotateExtra(ridable.RotThisFrame().y);
        }
    }

    /// <summary>
    /// Apply additional (artificially added) velocity for this frame.
    /// </summary>
    private void HandleAdditionalVelocity()
    {
        if (additionalVelocityToAdd.magnitude > 0)
        {
            transform.Translate(additionalVelocityToAdd * Time.deltaTime, Space.World);
            additionalVelocityToAdd = Vector3.zero;
        }
    }

    /// <summary>
    /// If the move this frame is different from what the move was previously,
    /// signal that the move has changed.
    /// </summary>
    private void HandleMovementChangeEvent()
    {
        if (moveThisFrame != moveLastFrame)
        {
            OnMoveChanged.Invoke(moveLastFrame, moveThisFrame);
            moveLastFrame = moveThisFrame;
        }
    }

    /// <summary>
    /// If there is an active checkpoint, teleports the player and camera target
    /// to it accordingly. Otherwise, does nothing.
    /// </summary>
    ///
    /*
    private void RespawnPlayerToCheckpoint()
    {
        CheckpointSystem cs = cl.GetCheckpoint();
        if (cs != null)
        {
            Vector3 goalPos = cs.GetPosition() + (Vector3.up * 1f);
            moveThisFrame = new Fall(mii, mi, movementSettings, Vector2.zero, false);
            charCont.enabled = false;
            transform.position = goalPos;
            charCont.enabled = true;
            camTarget.ResetToPlayerCenter();
        }
        else
        {
            Debug.LogError("No checkpoint active!");
        }
    }
    */

    /// <summary>
    /// Gives the direction of player movement based on the player's horizontal
    /// movement vector and the slope they're standing on.
    /// </summary>
    private Vector3 DirectionOfMovement(Vector2 horizMovement)
    {
        if (!moveThisFrame.AdjustToSlope() || PlayerSlopeHandler.ShouldSlide) return new Vector3(horizMovement.x, 0, horizMovement.y).normalized;

        float horizAngleFaced = Mathf.Atan2(horizMovement.y, horizMovement.x);

        float xDelta = Mathf.Cos(horizAngleFaced);
        float zDelta = Mathf.Sin(horizAngleFaced);
        float yDelta = (xDelta * PlayerSlopeHandler.XDeriv) + (zDelta * PlayerSlopeHandler.ZDeriv);

        // Fixing the quirks of y movement
        if (yDelta > 0) yDelta = 0; // CharacterController will take care of ascension

        Vector3 dir = new Vector3(xDelta, yDelta, zDelta);
        return dir;
    }

    /// <summary>
    /// Provides immutable access to the move which is currently taking place.
    /// </summary>
    public IMoveImmutable GetCurrentMove()
    {
        return moveThisFrame;
    }
}
