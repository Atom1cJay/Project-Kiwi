using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HorizontalMovement))]
[RequireComponent(typeof(MovementMaster))]
public class RotationMovement : MonoBehaviour
{
    [SerializeField] private float instantRotationSpeed = 0.2f;
    [SerializeField] private float groundRotationSpeed = 600;
    [SerializeField] private float airRotationSpeed = 200;
    private MovementMaster mm;
    private HorizontalMovement horizMovement;

    private void Awake()
    {
        mm = GetComponent<MovementMaster>();
        horizMovement = GetComponent<HorizontalMovement>();
    }

    private void FixedUpdate()
    {
        DetermineRotation();
    }

    /// <summary>
    /// Appropriately rotates the player considering the state they're in
    /// </summary>
    /// <param name="rawInput">The input whose direction will be rotated towards</param>
    private void DetermineRotation()
    {
        float rotationSpeed = mm.IsOnGround() ? groundRotationSpeed : airRotationSpeed;

        Vector2 rawInput = mm.GetHorizontalInput();
        if (rawInput.magnitude == 0) return;

        float camDirection = horizMovement.GetRelevantCamera().transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float inputDirection = Mathf.Atan2(rawInput.x, rawInput.y) + camDirection;
        Quaternion targetRotation = Quaternion.Euler(0, inputDirection * Mathf.Rad2Deg, 0);

        if (horizMovement.GetSpeed() <= instantRotationSpeed)
        {
            transform.rotation = targetRotation;
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
