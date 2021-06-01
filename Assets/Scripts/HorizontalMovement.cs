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
    [SerializeField] private float defaultMaxSpeed;
    [SerializeField] private float sensitivity;
    [SerializeField] private float gravity;
    [SerializeField] private float overTopSpeedGravity;
    [SerializeField] private float airSensitivity;
    [SerializeField] private float airGravity;
    [SerializeField] private float overTopSpeedAirGravity;
    [SerializeField] private float tjAirSensitivity;
    [SerializeField] private float tjAirGravity;
    [SerializeField] private float stickToGroundMultiplier = 0.2f;
    [SerializeField] private float hardTurnSpeedMultiplier;
    [SerializeField] private float airBoostSpeed;
    [SerializeField] private float airBoostChargeGravity;
    [SerializeField] private float vertAirBoostChargeGravity;
    [SerializeField] private float groundBoostSpeed;
    [SerializeField] private float groundBoostSensitivity;
    [SerializeField] private float groundBoostGravity;
    [SerializeField] private float diveSpeed;
    [SerializeField] private float airReverseSensitivity;
    [SerializeField] private float airReverseGravity;
    private float currentSpeed = 0;
    private MovementMaster mm;

    Vector3 amountToMove; // Update value in FixedUpdate, execute movement in Update

    private void Awake()
    {
        mm = GetComponent<MovementMaster>();
        mm.mm_OnHardTurnStart.AddListener(OnHardTurnStart);
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

    private void OnHardTurnStart()
    {
        currentSpeed *= this.hardTurnSpeedMultiplier;
    }

    private void DecideCurrentSpeed()
    {
        if (mm.IsAirDiving())
        {
            currentSpeed = new Dive(diveSpeed).GetHorizSpeedThisFrame();
        }
        else if (mm.IsGroundBoosting())
        {
            currentSpeed = InputUtils.SmoothedInput(currentSpeed, groundBoostSpeed, groundBoostSensitivity, groundBoostGravity);
        }
        else if (mm.InVertAirBoostCharge())
        {
            currentSpeed = InputUtils.SmoothedInput(currentSpeed, 0, 0, airBoostChargeGravity);
        }
        else if (mm.InAirBoostCharge() && mm.IsOnGround())
        {
            currentSpeed =
                new Run(
                    currentSpeed,
                    defaultMaxSpeed,
                    mm.GetHorizontalInput(),
                    sensitivity,
                    gravity,
                    overTopSpeedGravity).GetHorizSpeedThisFrame();

        }
        else if (mm.InAirBoostCharge() && !mm.IsOnGround())
        {
            currentSpeed = InputUtils.SmoothedInput(currentSpeed, 0, 0, airBoostChargeGravity);
        }
        else if (mm.InAirBoost())
        {
            currentSpeed = airBoostSpeed;
        }
        else if (mm.InTripleJump())
        {
            if (mm.IsAirReversing())
            {
                currentSpeed = InputUtils.SmoothedInput(currentSpeed, -mm.GetHorizontalInput().magnitude * defaultMaxSpeed, airReverseSensitivity, airReverseGravity);
                if (currentSpeed < 0) currentSpeed = 0;
            }
            else
            {
                currentSpeed = InputUtils.SmoothedInput(currentSpeed, mm.GetHorizontalInput().magnitude * defaultMaxSpeed, tjAirSensitivity, tjAirGravity);
            }
        }
        else if (!mm.IsInHardTurn() && mm.IsOnGround())
        {
            currentSpeed =
                new Run(
                    currentSpeed,
                    defaultMaxSpeed,
                    mm.GetHorizontalInput(),
                    sensitivity,
                    gravity,
                    overTopSpeedGravity).GetHorizSpeedThisFrame();
        }
        else if (!mm.IsInHardTurn() && !mm.IsOnGround())
        {
            if (mm.IsAirReversing())
            {
                currentSpeed = InputUtils.SmoothedInput(currentSpeed, -mm.GetHorizontalInput().magnitude * defaultMaxSpeed, airReverseSensitivity, airReverseGravity);
                if (currentSpeed < 0) currentSpeed = 0;
            }
            else if (currentSpeed > defaultMaxSpeed)
            {
                currentSpeed = InputUtils.SmoothedInput(currentSpeed, mm.GetHorizontalInput().magnitude * defaultMaxSpeed, sensitivity, overTopSpeedAirGravity);
            }
            else
            {
                currentSpeed = InputUtils.SmoothedInput(currentSpeed, mm.GetHorizontalInput().magnitude * defaultMaxSpeed, airSensitivity, airGravity);
            }
        }
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

}
