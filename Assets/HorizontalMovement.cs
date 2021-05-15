using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all movement unrelated to gravity and/or jumping.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class HorizontalMovement : MovementMaster
{
    [SerializeField] private GameObject relevantCamera;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float sensitivity;
    [SerializeField] private float gravity;
    [SerializeField] private float rotationSpeed = 800;
    [SerializeField] private float stickToGroundMultiplier = 0.2f;
    [SerializeField] private float instantRotationSpeed = 0.2f;
    private float currentSpeed = 0;

    /// <summary>
    /// Handles all the regular actions related to the player's horizontal movement.
    /// Updates the current speed, as well, for movement as well as rotation purposes.
    /// </summary>
    private void FixedUpdate()
    {
        DetermineRotation(HorizontalInput);
        currentSpeed = InputUtils.SmoothedInput(currentSpeed, HorizontalInput.magnitude * maxSpeed, sensitivity, gravity);
        MovePlayer();
    }

    /// <summary>
    /// Moves the player in the appropriate direction / speed.
    /// </summary>
    private void MovePlayer()
    {
        Vector3 dir = DirectionOfMovement();
        CharCont.Move(dir * currentSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Gives the direction of player movement based on the player's rotation
    /// and the slope they're standing on.
    /// </summary>
    /// <returns></returns>
    private Vector3 DirectionOfMovement()
    {
        if (IsJumping) return transform.forward;

        float horizAngleFaced = Mathf.Atan2(transform.forward.z, transform.forward.x);

        float xDelta = Mathf.Cos(horizAngleFaced);
        float zDelta = Mathf.Sin(horizAngleFaced);
        float yDelta = (xDelta * PlayerSlopeHandler.XDeriv) + (zDelta * PlayerSlopeHandler.ZDeriv);

        // Fixing the quirks of y movement
        if (yDelta > 0) yDelta = 0; // CharacterController will take care of ascension
        if (IsOnGround) yDelta -= Mathf.Abs(yDelta * stickToGroundMultiplier); // To keep player stuck to ground

        Vector3 dir = new Vector3(xDelta, yDelta, zDelta);
        return dir;
    }

    /// <summary>
    /// Given the horizontal input, appropriately rotates the player considering the state they're in
    /// </summary>
    /// <param name="rawInput">The input whose direction will be rotated towards</param>
    private void DetermineRotation(Vector2 rawInput)
    {
        if (rawInput.magnitude == 0) return;

        float camDirection = relevantCamera.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float inputDirection = Mathf.Atan2(rawInput.x, rawInput.y) + camDirection;
        Quaternion targetRotation = Quaternion.Euler(0, inputDirection * Mathf.Rad2Deg, 0);

        if (currentSpeed <= instantRotationSpeed)
        {
            transform.rotation = targetRotation;
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    // TODO should be put in a different class?
}
