using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class MovementMaster : UsesInputActions
{
    // Serialized Fields
    [SerializeField] private CollisionDetector groundDetector;
    [SerializeField] private float jumpEndableTimer = 0.1f;

    // Other variables for internal use only
    [HideInInspector] private bool jumpEndable; // Should the jump end if player is touching ground?

    // State Variables for Subclasses
    [HideInInspector] private bool isJumping;
    [HideInInspector] private bool jumpInputCanceled;
    [HideInInspector] private bool isOnGround;

    // Helpful Assets for Subclasses
    [HideInInspector] private CharacterController charCont;

    // UnityEvents
    [HideInInspector] public UnityEvent mm_OnJump;
    [HideInInspector] public UnityEvent mm_OnJumpCanceled;
    [HideInInspector] public UnityEvent mm_OnFirstFrameGrounded;
    [HideInInspector] public UnityEvent mm_FixedUpdateWhileInAir;
    [HideInInspector] public UnityEvent mm_FixedUpdateWhileGrounded;

    // INITIALIZATION /////////////////////////////////////////////////////////////////////////////////////////////////////////

    protected override void Awake2()
    {
        InitializeAssets();
        InitializeInputEvents();
    }

    private void InitializeAssets()
    {
        charCont = GetComponent<CharacterController>();
    }

    private void InitializeInputEvents()
    {
        inputActions.Player.Jump.performed += _ => OnJumpInputPerformed();
        inputActions.Player.Jump.canceled += _ => OnJumpInputCanceled();
    }

    // STATE CONTROL /////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void OnJumpInputPerformed()
    {
        if (isOnGround)
        {
            jumpInputCanceled = false;
            isJumping = true;
            jumpEndable = false;
            Invoke("MakeJumpEndable", jumpEndableTimer);
            mm_OnJump.Invoke();
        }
    }

    private void MakeJumpEndable()
    {
        jumpEndable = true;
    }

    private void OnJumpInputCanceled()
    {
        jumpInputCanceled = true;
        mm_OnJumpCanceled.Invoke();
    }

    private void FixedUpdate()
    {
        UpdateVerticalStates();
    }

    /// <summary>
    /// Decides what states the player is in terms of vertical movement,
    /// depending on whether the player is colliding with the ground, and current
    /// states related to jumping.
    /// </summary>
    private void UpdateVerticalStates()
    {
        bool touchingGround = groundDetector.Colliding();

        if (touchingGround && jumpEndable)
        {
            isJumping = false;
        }

        if (touchingGround && (!isJumping || jumpEndable))
        {
            if (!isOnGround)
            {
                isOnGround = true;
                mm_OnFirstFrameGrounded.Invoke();
            }
        }
        else
        {
            isOnGround = false;
        }

        if (isOnGround)
        {
            mm_FixedUpdateWhileGrounded.Invoke();
        }
        else
        {
            mm_FixedUpdateWhileInAir.Invoke();
        }
    }

    // GETTERS ////////////////////////////////////////////////////////////////////////////

    public bool IsJumping()
    {
        return isJumping;
    }

    public bool JumpInputCancelled()
    {
        return jumpInputCanceled;
    }

    public bool IsOnGround()
    {
        return isOnGround;
    }

    public CharacterController GetCharacterController()
    {
        return charCont;
    }

    /// <summary>
    /// Gives the normalized horizontal movement input.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetHorizontalInput()
    {
        Vector2 rawInput = inputActions.Player.Move.ReadValue<Vector2>();

        if (rawInput.magnitude > 1)
        {
            rawInput = rawInput.normalized;
        }

        return rawInput;
    }
}
