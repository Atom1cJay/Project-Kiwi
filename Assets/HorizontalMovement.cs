using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all horizontal movement, and any input related to horizontal movement
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class HorizontalMovement : MovementMaster
{
    [SerializeField] private GameObject cameraFacingMe;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float sensitivity;
    [SerializeField] private float gravity;
    [SerializeField] private float rotationSpeed = 800;
    [SerializeField] private float extraDownMultiplierOnGround;
    private float currentSpeed = 0;
    private bool isStopped = true;

    /// <summary>
    /// Handles all the regular actions related to the player's horizontal movement.
    /// </summary>
    private void FixedUpdate()
    {
        Vector2 rawInput = GetRawHorizontalInput();

        DetermineRotation(rawInput);

        currentSpeed = InputUtils.SmoothedInput(currentSpeed, rawInput.magnitude * maxSpeed, sensitivity, gravity);
        isStopped = currentSpeed == 0;

        MovePlayer();
    }

    /// <summary>
    /// Moves the player in the appropriate direction / speed.
    /// </summary>
    private void MovePlayer()
    {
        if (IsJumping)
        {
            CharCont.Move(transform.forward * currentSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 dir = directionOfMovement();
            CharCont.Move(dir * currentSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Gives the normalized horizontal movement input.
    /// </summary>
    /// <returns></returns>
    private Vector2 GetRawHorizontalInput()
    {
        Vector2 rawInput = inputActions.Player.Move.ReadValue<Vector2>();

        if (rawInput.magnitude > 1)
        {
            rawInput = rawInput.normalized;
        }

        return rawInput;
    }

    /// <summary>
    /// Gives the normalized direction of movement based on the player's rotation
    /// and the slope they're standing on.
    /// </summary>
    /// <returns></returns>
    private Vector3 directionOfMovement()
    {
        float horizAngleFaced = Mathf.Atan2(transform.forward.z, transform.forward.x);

        float xDelta = Mathf.Cos(horizAngleFaced);
        float zDelta = Mathf.Sin(horizAngleFaced);
        float yDelta = (xDelta * PlayerSlopeHandler.XDeriv) + (zDelta * PlayerSlopeHandler.ZDeriv);
        if (yDelta > 0) yDelta = 0;

        Vector3 dir = new Vector3(xDelta, yDelta, zDelta);

        if (IsOnGround)
        {
            dir -= new Vector3(0, Mathf.Abs(dir.y * extraDownMultiplierOnGround), 0);
        }

        return dir;
    }

    /// <summary>
    /// Progressively rotates the player towards the direction of input
    /// </summary>
    /// <param name="rawInput">The input whose direction will be rotated towards</param>
    private void DetermineRotation(Vector2 rawInput)
    {
        float camDirection = cameraFacingMe.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float inputDirection = Mathf.Atan2(rawInput.x, rawInput.y) + camDirection;
        Quaternion targetRotation = Quaternion.Euler(0, inputDirection * Mathf.Rad2Deg, 0);

        if (isStopped && rawInput.magnitude > 0)
        {
            transform.rotation = targetRotation;
        }
        else if (rawInput.magnitude > 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    // TODO should be put in a different class?
}
