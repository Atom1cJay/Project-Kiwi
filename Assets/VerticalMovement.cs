using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementMaster))]
[System.Serializable]
public class VerticalMovement : MonoBehaviour
{
    [SerializeField] private float initJumpVel;
    [SerializeField] private float initGravity;
    [SerializeField] private float maxGravity;
    [SerializeField] private float maxGravityAtCancel;
    [SerializeField] private float velocityMultiplierAtCancel = 0.5f;
    [SerializeField] private float gravityIncRate;
    [SerializeField] private float gravityIncRateAtCancel;
    [SerializeField] private float nonJumpGravity;
    private float gravity;
    private float vertVel;
    private MovementMaster mm;

    private void Awake()
    {
        mm = GetComponent<MovementMaster>();
        mm.mm_OnJump.AddListener(OnJump);
        mm.mm_OnJumpCanceled.AddListener(OnJumpCanceled);
        mm.mm_OnFirstFrameGrounded.AddListener(OnFirstFrameGrounded);
        mm.mm_FixedUpdateWhileGrounded.AddListener(FixedUpdateWhileOnGround);
    }

    private void Start()
    {
        gravity = nonJumpGravity;
    }

    /// <summary>
    /// Initiates a jump if the player is touching the ground.
    /// </summary>
    private void OnJump()
    {
        StopCoroutine("Jump");
        StartCoroutine("Jump");
    }

    /// <summary>
    /// Marks a jump as cancelled (meaning the arc should begin to decrease)
    /// if we are in the proper part of the jump.
    /// </summary>
    private void OnJumpCanceled()
    {
        // TODO point of no return
        if (vertVel > 0)
        {
            vertVel *= velocityMultiplierAtCancel;
        }
    }

    /// <summary>
    /// Manipulates the gravity (and, initially, the vertical velocity) in order
    /// to simulate a jump. If the jump is cancelled, modifies the arc
    /// the player is on.
    /// </summary>
    private IEnumerator Jump()
    {
        gravity = initGravity;
        vertVel = initJumpVel;

        while(mm.IsJumping())
        {
            if (mm.JumpInputCancelled())
                gravity += gravityIncRateAtCancel * Time.deltaTime;
            else
                gravity += gravityIncRate * Time.deltaTime;

            if (gravity > maxGravity && !mm.JumpInputCancelled())
                gravity = maxGravity;
            else if (gravity > maxGravityAtCancel && mm.JumpInputCancelled())
                gravity = maxGravityAtCancel;

            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Handle final movement for each frame
    /// </summary>
    private void Update()
    {
        EnforceGravity();
        MoveByVertVel();
    }

    /// <summary>
    /// Do stuff for the first frame the ground is touched
    /// </summary>
    private void OnFirstFrameGrounded()
    {
        // Ensure player makes direct contact ground upon hitting it
        mm.GetCharacterController().Move(new Vector3(0, -1, 0));
    }

    /// <summary>
    /// Stuff to do while on the ground
    /// </summary>
    private void FixedUpdateWhileOnGround()
    {
        gravity = nonJumpGravity;
        vertVel = 0;
    }

    /// <summary>
    /// If the player is jumping, adjusts velocity depending on the gravity, and moves based on the resulting velocity
    /// </summary>
    private void EnforceGravity()
    {
        if (!mm.IsOnGround())
        {
            vertVel -= gravity * Time.deltaTime;
        }
    }

    /// <summary>
    /// Moves the player by the current vertical velocity
    /// </summary>
    private void MoveByVertVel()
    {
        mm.GetCharacterController().Move(new Vector3(0, vertVel * Time.deltaTime, 0));
    }
}
