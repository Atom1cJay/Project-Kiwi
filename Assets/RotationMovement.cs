using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HorizontalMovement))]
[RequireComponent(typeof(MovementMaster))]
public class RotationMovement : MonoBehaviour
{
    [SerializeField] private float instantRotationSpeed = 0.2f;
    //[SerializeField] private float hardTurnRotSeverity = 1f;
    //[SerializeField] private float hardTurnSpeed = 1f;
    [SerializeField] private float groundRotationSpeed = 600;
    [SerializeField] private float airRotationSpeed = 200;
    private MovementMaster mm;
    //private HorizontalMovement horizMovement;
    private float directionFacing; // In radians

    private void Awake()
    {
        mm = GetComponent<MovementMaster>();
        //horizMovement = GetComponent<HorizontalMovement>();
        mm.mm_OnHardTurnEnd.AddListener(OnHardTurnEnd);
    }

    private void FixedUpdate()
    {
        directionFacing = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        DetermineRotation();
    }

    /// <summary>
    /// Appropriately rotates the player considering the state they're in
    /// </summary>
    /// <param name="rawInput">The input whose direction will be rotated towards</param>
    private void DetermineRotation()
    {
        // Get basic direction info
        Vector2 rawInput = mm.GetHorizontalInput();
        float camDirection = mm.GetRelevantCamera().transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float inputDirection = Mathf.Atan2(rawInput.x, rawInput.y) + camDirection;

        if (!mm.IsInHardTurn())
        {
            float rotationSpeed = mm.IsOnGround() ? groundRotationSpeed : airRotationSpeed;
            if (rawInput.magnitude == 0) return;
            Quaternion targetRotation = Quaternion.Euler(0, inputDirection * Mathf.Rad2Deg, 0);
            bool underInstantRotSpeed = mm.GetHorizSpeed() <= instantRotationSpeed;
            transform.rotation = underInstantRotSpeed ? targetRotation : Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Changes the rotation appropriately for the end of a hard turn.
    /// </summary>
    private void OnHardTurnEnd()
    {
        Vector2 rawInput = mm.GetHorizontalInput();
        if (rawInput.magnitude == 0) return;
        float camDirection = mm.GetRelevantCamera().transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float inputDirection = Mathf.Atan2(rawInput.x, rawInput.y) + camDirection;
        Quaternion targetRotation = Quaternion.Euler(0, inputDirection * Mathf.Rad2Deg, 0);
        transform.rotation = targetRotation;
    }
}
