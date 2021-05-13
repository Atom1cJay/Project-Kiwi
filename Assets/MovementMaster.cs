using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementMaster : UsesInputActions
{
    // Description in Editor
    private static readonly float jumpEndableTimer = 0.1f;

    // Other variables for internal use only
    private static bool jumpEndable; // Should the jump end if player is touching ground?

    // State Variables for Subclasses
    protected static bool IsJumping { get; private set; }
    protected static bool JumpInputCanceled { get; private set; }
    protected static bool IsOnGround {get; private set; }

    // Helpful Assets for Subclasses
    protected static CharacterController CharCont { get; private set; }
    protected static PlayerGroundCollisionDetector GroundDetector { get; private set; }

    // INITIALIZATION

    protected sealed override void Awake2()
    {
        InitializeAssets();
        InitializeInputEvents();
    }

    private void InitializeAssets()
    {
        GroundDetector = PlayerGroundCollisionDetector.Instance;
        CharCont = GetComponent<CharacterController>();
    }

    private void InitializeInputEvents()
    {
        inputActions.Player.Jump.performed += _ => OnJumpInputPerformed();
        inputActions.Player.Jump.canceled += _ => OnJumpInputCanceled();
    }

    // STATE CONTROL

    private void OnJumpInputPerformed()
    {
        if (IsOnGround)
        {
            JumpInputCanceled = false;
            IsJumping = true;
            jumpEndable = false;
            Invoke("makeJumpEndable", jumpEndableTimer);
            OnJump();
        }
    }

    private void makeJumpEndable()
    {
        jumpEndable = true;
    }

    private void OnJumpInputCanceled()
    {
        JumpInputCanceled = true;
        OnJumpCanceled();
    }

    private void FixedUpdate()
    {
        bool touchingGround = GroundDetector.Colliding();

        if (touchingGround && jumpEndable)
        {
            IsJumping = false;
        }

        if (touchingGround && (!IsJumping || jumpEndable))
        {
            if (!IsOnGround)
            {
                IsOnGround = true;
                OnFirstFrameGrounded();
            }
        }
        else
        {
            IsOnGround = false;
        }

        if (IsOnGround)
        {
            FixedUpdateWhileOnGround();
        }
        else
        {
            FixedUpdateWhileInAir();
        }
    }

    // OVERRIDABLE METHODS

    /// <summary>
    /// KEEP THIS EMPTY IN THIS CLASS!
    /// Override this in any subclasses if you want to do something the frame the input for jumping is performed
    /// </summary>
    protected virtual void OnJump() { }

    /// <summary>
    /// KEEP THIS EMPTY IN THIS CLASS!
    /// Override this in any subclasses if you want to do something the frame the input for jumping is canceled
    /// </summary>
    protected virtual void OnJumpCanceled() { }

    /// <summary>
    /// KEEP THIS EMPTY IN THIS CLASS!
    /// Override this in any subclasses if you want to do something the frame the player gets grounded
    /// </summary>
    protected virtual void OnFirstFrameGrounded() { }

    /// <summary>
    /// KEEP THIS EMPTY IN THIS CLASS!
    /// Override this in any subclasses if you want to do something while the player is in the air.
    /// Uses the FixedUpdate timestep
    /// </summary>
    protected virtual void FixedUpdateWhileInAir() { }

    /// <summary>
    /// KEEP THIS EMPTY IN THIS CLASS!
    /// Override this in any subclasses if you want to do something while the player is on the ground.
    /// Uses the FixedUpdate timestep
    /// </summary>
    protected virtual void FixedUpdateWhileOnGround() { }
}
