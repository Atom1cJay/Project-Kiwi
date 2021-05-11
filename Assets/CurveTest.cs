using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[System.Serializable]
public class CurveTest : UsesInputActions
{
    [SerializeField] CollisionDetector detectsGround;
    [SerializeField] private float gravity; // Should be negative. Serialized: the default value
    [SerializeField] private float initJumpVel;
    [SerializeField] private float initGravity;
    [SerializeField] private float maxGravity;
    [SerializeField] private float maxGravityAtCancel;
    [SerializeField] private float velocityMultiplierAtCancel = 0.5f;
    [SerializeField] private float gravityIncRate;
    [SerializeField] private float gravityIncRateAtCancel;
    [SerializeField] private float nonJumpGravity;
    private CharacterController charCont;
    private bool touchingSurface;
    private float vertVel;
    private bool jumpCancelled;

    protected override void Awake2()
    {
        charCont = GetComponent<CharacterController>();
        inputActions.Player.Jump.performed += _ => PerformJump();
        inputActions.Player.Jump.canceled += _ => CancelJump();
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
    /// Marks a jump as cancelled if we are in the proper part of the jump.
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
        jumpCancelled = false;
        gravity = initGravity;
        vertVel = initJumpVel;

        while(!touchingSurface)
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

        gravity = 0;
    }

    private void FixedUpdate()
    {
        if (detectsGround.Colliding())
        {
            gravity = nonJumpGravity;
        }
    }

    private void Update()
    {
        EnforceGravity();
    }

    /// <summary>
    /// Adjusts velocity depending on the gravity, and moves based on the resulting velocity
    /// </summary>
    private void EnforceGravity()
    {
        vertVel -= gravity * Time.deltaTime;
        charCont.Move(new Vector3(0, vertVel * Time.deltaTime, 0));
    }
}
