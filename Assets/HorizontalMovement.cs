using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all horizontal movement, and any input related to horizontal movement
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class HorizontalMovement : UsesInputActions
{
    [SerializeField] private GameObject cameraFacingMe;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float sensitivity;
    [SerializeField] private float gravity;
    [SerializeField] private float rotationSpeed = 800;
    private CharacterController charCont;
    private float currentSpeed = 0;
    private bool isStopped = true;

    protected override void Awake2()
    {
        charCont = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        // Get raw input in Cartesian coords
        Vector2 rawInput = inputActions.Player.Move.ReadValue<Vector2>();
        // In case there's any weirdness in the input device
        if (rawInput.magnitude > 1) rawInput = rawInput.normalized;

        // Change Rotation Appropriately
        DetermineRotation(rawInput);

        // Get Speed
        currentSpeed = InputUtils.SmoothedInput(currentSpeed, rawInput.magnitude * maxSpeed, sensitivity, gravity);
        isStopped = currentSpeed == 0;

        // Enact Movement
        charCont.Move(transform.forward * currentSpeed * Time.fixedDeltaTime);
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
