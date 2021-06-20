using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all movement unrelated to gravity and/or jumping.
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MovementMaster))]
public class HorizontalMovement : MonoBehaviour
{
    [SerializeField] public float defaultMaxSpeed;
    [SerializeField] public float sensitivity;
    [SerializeField] public float gravity;
    [SerializeField] public float overTopSpeedGravity;
    [SerializeField] public float airSensitivity;
    [SerializeField] public float airGravity;
    [SerializeField] public float overTopSpeedAirGravity;
    [SerializeField] public float tjAirSensitivity;
    [SerializeField] public float tjAirGravity;
    [SerializeField] public float stickToGroundMultiplier;
    [SerializeField] public float hardTurnGravity;
    [SerializeField] public float airBoostSpeed;
    [SerializeField] public float airBoostChargeGravity;
    [SerializeField] public float vertAirBoostChargeGravity;
    [SerializeField] public float groundBoostSpeed;
    [SerializeField] public float groundBoostSensitivity;
    [SerializeField] public float groundBoostGravity;
    [SerializeField] public float diveSpeed;
    [SerializeField] public float airReverseSensitivity;
    [SerializeField] public float airReverseGravity;
    public float currentSpeed;
    private MovementMaster mm;
    private VerticalMovement vm;

    Vector3 amountToMove; // Update value in FixedUpdate, execute movement in Update

    private void Awake()
    {
        mm = GetComponent<MovementMaster>();
        vm = GetComponent<VerticalMovement>();
    }

    private void Update()
    {
        mm.GetCharacterController().Move(amountToMove * Time.deltaTime);
    }

    /// <summary>
    /// Handles all the regular actions related to the player's horizontal movement.
    /// Updates the current speed, as well, for movement as well as rotation purposes.
    /// </summary>
    private void FixedUpdate()
    {
        amountToMove = Vector3.zero;
        DecideCurrentSpeed();
        MovePlayer();
    }

    /// <summary>
    /// Based on the current move that is apparently taking place, decides what the horizontal
    /// speed should be.
    /// </summary>
    private void DecideCurrentSpeed()
    {
        IMove moveBeingPerformed = null;

        if (mm.IsAirDiving())
        {
            moveBeingPerformed = new Dive(this, vm, mm);
        }
        else if (mm.IsGroundBoosting())
        {
            moveBeingPerformed = new HorizGroundBoost(this, vm, mm);
        }
        else if (mm.InVertAirBoostCharge())
        {
            moveBeingPerformed = new VertAirBoostCharge(this, vm, mm, 0); // 0 temp
        }
        else if (mm.InAirBoostCharge() && !mm.IsOnGround())
        {
            moveBeingPerformed = new HorizAirBoostCharge(this, vm, mm, 0); // 0 temp
        }
        else if (mm.InAirBoost())
        {
            moveBeingPerformed = new HorizAirBoost(this, vm, mm);
        }
        else if (mm.InTripleJump())
        {
            moveBeingPerformed = new TripleJump(this, vm, mm);
        }
        else if (mm.IsInHardTurn())
        {
            moveBeingPerformed = new HardTurn(this, vm, mm);
        }
        else if (!mm.IsInHardTurn() && mm.IsOnGround())
        {
            moveBeingPerformed = new Run(this, vm, mm);
        }
        else if (!mm.IsInHardTurn() && !mm.IsOnGround())
        {
            moveBeingPerformed = new Jump(this, vm, mm);
        }
        else
        {
            Debug.LogError("Impossible horizontal move: none of the other moves' conditions passed.");
        }

        currentSpeed = moveBeingPerformed.GetHorizSpeedThisFrame();
    }

    /// <summary>
    /// Moves the player in the appropriate direction / speed.
    /// </summary>
    private void MovePlayer()
    {
        Vector3 dir = DirectionOfMovement();
        amountToMove += (dir * currentSpeed);
    }

    /// <summary>
    /// Gives the direction of player movement based on the player's rotation
    /// and the slope they're standing on.
    /// </summary>
    private Vector3 DirectionOfMovement()
    {
        if (mm.IsJumping()) return transform.forward;

        float horizAngleFaced = Mathf.Atan2(transform.forward.z, transform.forward.x);

        float xDelta = Mathf.Cos(horizAngleFaced);
        float zDelta = Mathf.Sin(horizAngleFaced);
        float yDelta = (xDelta * PlayerSlopeHandler.XDeriv) + (zDelta * PlayerSlopeHandler.ZDeriv);

        // Fixing the quirks of y movement
        if (yDelta > 0) yDelta = 0; // CharacterController will take care of ascension
        if (mm.IsOnGround()) yDelta -= Mathf.Abs(yDelta * stickToGroundMultiplier); // To keep player stuck to ground

        Vector3 dir = new Vector3(xDelta, yDelta, zDelta);
        if (dir.magnitude > 1) { dir = dir.normalized; }
        return dir;
    }

    /// <summary>
    /// Returns the speed at which the player is currently moving in any
    /// horizontal-related movement.
    /// </summary>
    public float GetSpeed()
    {
        return currentSpeed;
    }

    /// <summary>
    /// Gives the horizontal input as given by the movement master
    /// </summary>
    /// <returns></returns>
    public Vector2 getHorizontalInput()
    {
        return mm.GetHorizontalInput();
    }

    /// <summary>
    /// Determines whether an air reverse input is being made right now
    /// </summary>
    /// <returns></returns>
    public bool isAirReversing()
    {
        return mm.IsAirReversing();
    }
}
