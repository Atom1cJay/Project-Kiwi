using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(HorizontalMovement))]
[RequireComponent(typeof(CharacterController))]
public class MovementMaster : UsesInputActions
{
    // Serialized Fields
    [SerializeField] private GameObject relevantCamera;
    [SerializeField] private CollisionDetector groundDetector;
    [SerializeField] private float jumpEndableTimer = 0.1f;
    [SerializeField] private float reverseCoyoteTime;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float dissonanceForHardTurn;
    [SerializeField] private float hardTurnMinSpeed;
    [SerializeField] private float hardTurnTime;

    // Other variables for internal use only
    private bool isJumping;
    private bool jumpEndable; // Should the jump end if player is touching ground?
    private bool inCoyoteTime;
    private bool jumpInputCanceled;
    private bool isOnGround;
    private bool isInHardTurn;

    // Helpful Assets for Subclasses
    private CharacterController charCont;
    private HorizontalMovement horizMove;

    // UnityEvents
    [HideInInspector] public UnityEvent mm_OnJump;
    [HideInInspector] public UnityEvent mm_OnJumpCanceled;
    [HideInInspector] public UnityEvent mm_OnFirstFrameGrounded;
    [HideInInspector] public UnityEvent mm_FixedUpdateWhileInAir;
    [HideInInspector] public UnityEvent mm_FixedUpdateWhileGrounded;
    [HideInInspector] public UnityEvent mm_OnHardTurnStart;
    [HideInInspector] public UnityEvent mm_OnHardTurnEnd;

    // INITIALIZATION /////////////////////////////////////////////////////////////////////////////////////////////////////////

    protected override void Awake2()
    {
        InitializeAssets();
        InitializeInputEvents();
    }

    private void InitializeAssets()
    {
        charCont = GetComponent<CharacterController>();
        horizMove = GetComponent<HorizontalMovement>();
    }

    private void InitializeInputEvents()
    {
        inputActions.Player.Jump.performed += _ => OnJumpInputPerformed();
        inputActions.Player.Jump.canceled += _ => OnJumpInputCanceled();
    }

    // STATE CONTROL /////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void OnJumpInputPerformed()
    {
        if (isOnGround || inCoyoteTime)
        {
            Jump();
        }

        if (!isOnGround && !inCoyoteTime)
        {
            StartCoroutine("ReverseCoyoteTime");
        }
    }

    private void Jump()
    {
        jumpInputCanceled = false;
        isJumping = true;
        jumpEndable = false;
        Invoke("MakeJumpEndable", jumpEndableTimer);
        mm_OnJump.Invoke();
    }

    IEnumerator ReverseCoyoteTime()
    {
        float timePassed = 0f;

        while (timePassed < reverseCoyoteTime)
        {
            bool pressingJump = inputActions.Player.Jump.ReadValue<float>() > 0;

            if (isOnGround && pressingJump)
            {
                Jump();
                timePassed = reverseCoyoteTime;
            }

            if (!pressingJump)
            {
                timePassed = reverseCoyoteTime;
            }

            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
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
        UpdateHorizontalStates();
    }

    private void UpdateVerticalStates()
    {
        bool touchingGround = groundDetector.Colliding();

        if (touchingGround && jumpEndable)
        {
            isJumping = false;
        }

        // Decide if is on ground
        if (touchingGround && (!isJumping || jumpEndable))
        {
            // On ground

            if (!isOnGround)
            {
                // First frame on ground
                isOnGround = true;
                mm_OnFirstFrameGrounded.Invoke();
            }
        }
        else
        {
            // Off ground

            if (isOnGround)
            {
                // First frame off ground
                isOnGround = false;

                if (!isJumping)
                {
                    StartCoroutine("CoyoteTime");
                }
            }
        }

        // Invoke things based on whether is on ground
        if (isOnGround)
        {
            mm_FixedUpdateWhileGrounded.Invoke();
        }
        else
        {
            mm_FixedUpdateWhileInAir.Invoke();
        }
    }

    IEnumerator CoyoteTime()
    {
        float timePassed = 0f;
        inCoyoteTime = true;

        while (timePassed < coyoteTime && !isJumping)
        {
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        inCoyoteTime = false;
    }

    private void UpdateHorizontalStates()
    {
        if (GetHorizDissonance() > dissonanceForHardTurn && horizMove.GetSpeed() > hardTurnMinSpeed && !isInHardTurn && isOnGround)
        {
            // First frame of hard turn
            StartHardTurn();
        }
    }

    private void StartHardTurn()
    {
        isInHardTurn = true;
        mm_OnHardTurnStart.Invoke();
        Invoke("EndHardTurn", hardTurnTime);
    }

    private void EndHardTurn()
    {
        isInHardTurn = false;
        mm_OnHardTurnEnd.Invoke();
    }

    /// <summary>
    /// Gives the distance (min = 0, max = PI) between the direction the player is facing and the
    /// direction of horizontal input
    /// </summary>
    /// <returns></returns>
    private float GetHorizDissonance()
    {
        Vector2 rawInput = GetHorizontalInput();
        float camDirection = relevantCamera.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float directionFacing = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float inputDirection = Mathf.Atan2(rawInput.x, rawInput.y) + camDirection;
        float inputVsFacing = Mathf.PI - Mathf.Abs(Mathf.Abs((inputDirection - directionFacing) % (2 * Mathf.PI)) - Mathf.PI);
        return inputVsFacing;
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

    public float GetHorizSpeed()
    {
        return horizMove.GetSpeed();
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

    /// <summary>
    /// Gives the ground detector being used in player movement calculations.
    /// </summary>
    /// <returns>The ground detector being used by the player.</returns>
    public CollisionDetector GetGroundDetector()
    {
        return groundDetector;
    }

    /// <summary>
    /// Is the player in the middle of a hard turn?
    /// </summary>
    public bool IsInHardTurn()
    {
        return isInHardTurn;
    }

    /// <summary>
    /// Gives the camera being used in player movement calculations.
    /// </summary>
    public GameObject GetRelevantCamera()
    {
        return relevantCamera;
    }
}
