using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[System.Serializable]
public class CurveTest : UsesInputActions
{
    [SerializeField] CollisionDetector detectsGround;
    [SerializeField] private float initJumpVel;
    [SerializeField] private float initGravity;
    [SerializeField] private float maxGravity;
    [SerializeField] private float maxGravityAtCancel;
    [SerializeField] private float velocityMultiplierAtCancel = 0.5f;
    [SerializeField] private float gravityIncRate;
    [SerializeField] private float gravityIncRateAtCancel;
    [SerializeField] private float nonJumpGravity;
    private float gravity;
    private CharacterController charCont;
    private float vertVel;
    private bool jumping;
    private bool jumpCancelled;
    private bool canBeLanded;

    /// <summary>
    /// Initialization
    /// </summary>
    protected override void Awake2()
    {
        charCont = GetComponent<CharacterController>();
        inputActions.Player.Jump.performed += _ => PerformJump();
        inputActions.Player.Jump.canceled += _ => CancelJump();
        gravity = nonJumpGravity;
    }

    /// <summary>
    /// Initiates a jump if the player is touching the ground.
    /// </summary>
    private void PerformJump()
    {
        if (detectsGround.Colliding())
        {
            StopAllCoroutines();
            StartCoroutine("Jump");
        }
    }

    /// <summary>
    /// Marks a jump as cancelled (meaning the arc should begin to decrease)
    /// if we are in the proper part of the jump.
    /// </summary>
    private void CancelJump()
    {
        // TODO point of no return
        if (vertVel > 0)
        {
            vertVel *= velocityMultiplierAtCancel;
            jumpCancelled = true;
        }
    }

    /// <summary>
    /// Manipulates the gravity (and, initially, the vertical velocity) in order
    /// to simulate a jump. If the jump is cancelled, modifies the arc
    /// the player is on.
    /// </summary>
    private IEnumerator Jump()
    {
        jumping = true;
        canBeLanded = false;
        Invoke("MakeLandable", 0.1f); // TODO better way to handle this?
        jumpCancelled = false;
        gravity = initGravity;
        vertVel = initJumpVel;

        while(!canBeLanded || !detectsGround.Colliding())
        {
            if (jumpCancelled)
                gravity += gravityIncRateAtCancel * Time.deltaTime;
            else
                gravity += gravityIncRate * Time.deltaTime;

            if (gravity > maxGravity && !jumpCancelled)
                gravity = maxGravity;
            else if (gravity > maxGravityAtCancel && jumpCancelled)
                gravity = maxGravityAtCancel;
            yield return new WaitForFixedUpdate();
        }

        GroundVelocity();
        jumping = false;
    }

    /// <summary>
    /// Sets the gravity and velocity appropriately for a player
    /// that has just grounded.
    /// </summary>
    private void GroundVelocity()
    {
        gravity = nonJumpGravity;
        vertVel = 0;
    }

    /// <summary>
    /// Makes it possible for the jump sequence to end (consider the player
    /// "landed" when it hits the ground")
    /// </summary>
    private void MakeLandable()
    {
        canBeLanded = true;
    }

    /// <summary>
    /// Handle final movement for each frame
    /// </summary>
    private void Update()
    {
        if (detectsGround.Colliding() && !jumping)
        {
            GroundVelocity();
        }
        EnforceGravity();
        MoveByVertVel();
    }

    /// <summary>
    /// If the player is jumping, adjusts velocity depending on the gravity, and moves based on the resulting velocity
    /// </summary>
    private void EnforceGravity()
    {
        if (!detectsGround.Colliding())
        {
            vertVel -= gravity * Time.deltaTime;
        }
    }

    /// <summary>
    /// Moves by the current vertical velocity
    /// </summary>
    private void MoveByVertVel()
    {
        charCont.Move(new Vector3(0, vertVel * Time.deltaTime, 0));
    }
}
