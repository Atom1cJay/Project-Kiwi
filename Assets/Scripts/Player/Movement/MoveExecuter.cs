using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Decides how the player and camera should move, depending on the currently
/// active IMove.
/// </summary>
[RequireComponent(typeof(MovementInfo))]
[RequireComponent(typeof(MovementInputInfo))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(RotationMovement))]
[RequireComponent(typeof(CheckpointLoader))]
[RequireComponent(typeof(BumperHandler))]
public class MoveExecuter : MonoBehaviour
{
    IMove moveLastFrame = null;
    IMove moveThisFrame;
    CharacterController charCont;
    MovementInfo mi;
    MovementInputInfo mii;
    MovementSettingsSO movementSettings;
    RotationMovement rotator;
    CheckpointLoader cl;
    BumperHandler bh;
    [SerializeField] CameraControl cameraControl;
    [SerializeField] CameraTarget camTarget;
    [SerializeField] StickToGround shadowCaster;
    [SerializeField] float barrierRadius;
    Ridable ridable = null;

    public delegate void OnMoveChangedDelegate(IMoveImmutable oldMove, IMoveImmutable newMove);

    public static event OnMoveChangedDelegate OnMoveChanged;

    private void Awake()
    {
        charCont = GetComponent<CharacterController>();
        mi = GetComponent<MovementInfo>();
        mii = GetComponent<MovementInputInfo>();
        cl = GetComponent<CheckpointLoader>();
        rotator = GetComponent<RotationMovement>();
        bh = GetComponent<BumperHandler>();
        Physics.autoSimulation = false;
    }

    private void Start()
    {
        movementSettings = MovementSettingsSO.Instance;
        moveThisFrame = new Fall(mii, mi, movementSettings, Vector2.zero, false);
        mii.OnRespawnToCheckpointInput.AddListener(() => RespawnPlayerToCheckpoint());
    }

    void Update()
    {
        if (ridable == null) // If not null, the moving platform's TransformChange event will call it instead
        {
            Move();
        }
    }

    private void Move()
    {
        HandleBasicMovement();
        HandleMovementChangeEvent();
        UpdateMovingPlatformStatus();
    }

    void UpdateMovingPlatformStatus()
    {
        if (ridable == null && mi.TouchingGround() && mi.GetGroundDetector().CollidingWith().CompareTag("Smooth Moving Platform (EXP)"))
        {
            ridable = mi.GetGroundDetector().CollidingWith().GetComponentInParent<Ridable>();
            ridable.Register();
            ridable.onTransformChange.AddListener(() => Move());
        }
        if (!moveThisFrame.AdjustToSlope() || (!mi.TouchingGround() && !PlayerSlopeHandler.GroundInProximity) ||(mi.TouchingGround() && !mi.GetGroundDetector().CollidingWith().CompareTag("Smooth Moving Platform (EXP)")))
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
    /// Handles horizontal, vertical, and rotation-related movement for the player,
    /// as well as the controls for the camera, depending
    /// on the currently active IMove.
    /// Also moves according to the current moving platform, if there is one.
    /// </summary>
    private void HandleBasicMovement()
    {
        if (Time.timeScale > 0 && Time.deltaTime > 0) // Check for the sake of avoiding weird errors
        {
            Vector2 origPosXZ = new Vector2(transform.position.x, transform.position.z);
            Vector3 extraMovement = Vector3.zero; // Movement from moving platform, if applicable. Should not pay attention to physics
            // Special Moving Platform Movement
            if (ridable != null)
            {
                extraMovement = ridable.TranslationThisFrame();
            }
            transform.Translate(extraMovement, Space.World);
            if (ridable != null)
            {
                transform.Translate(ridable.PlayerPosChangeFromRotThisFrame(new Vector3(transform.position.x, charCont.bounds.min.y, transform.position.z)), Space.World);
                rotator.RotateExtra(ridable.RotThisFrame().y);
            }
            // End Special Moving Platform Movement
            bh.HandleBumperMoved();
            Physics.Simulate(Time.deltaTime);
            bh.HandleBumperMoved();

            // BARRIER
            charCont.Move(Vector3.left * barrierRadius);
            charCont.Move(Vector3.back * barrierRadius);
            charCont.Move(Vector3.right * barrierRadius);
            charCont.Move(Vector3.forward * barrierRadius);

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
    private void RespawnPlayerToCheckpoint()
    {
        CheckpointSystem cs = cl.GetCheckpoint();
        if (cs != null)
        {
            Vector3 goalPos = cs.GetPosition() + (Vector3.up * 3f);
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
