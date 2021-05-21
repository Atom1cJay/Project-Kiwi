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
    [Header("Jumping Settings")]
    [SerializeField] private float jumpEndableTimer = 0.1f;
    [Header("Coyote Time Settings")]
    [SerializeField] private float reverseCoyoteTime;
    [SerializeField] private float coyoteTime;
    [Header("Hard Turn Settings")]
    [SerializeField] private float dissonanceForHardTurn;
    [SerializeField] private float hardTurnMinSpeed;
    [SerializeField] private float hardTurnTime;
    [Header("Triple Jump Settings")]
    [SerializeField] private float tjMinInputMagnitude;
    [SerializeField] private float tjMaxTimeBtwnJumps;
    [SerializeField] private float tjMinJumpTime;
    [SerializeField] private float tjMaxJumpTime;
    [SerializeField] private float tjMaxDissonance;
    [Header("Boosting Settings")]
    [SerializeField] private float airBoostMaxChargeTime;
    [SerializeField] private float airBoostMaxTime;
    [Header("Misc. Settings")]
    [SerializeField] private float dissonanceForAirReverse;

    // Other variables for internal use only
    private bool isJumping;
    private bool jumpEndable; // Should the jump end if player is touching ground?
    private bool inCoyoteTime;
    private bool jumpInputCanceled;
    private bool isOnGround;
    private bool isInHardTurn;
    private float tjCurJumpTime; // The elapsed time for the current jump. 0 if there is no jump happening.
    private float tjTimeBtwnJumps;
    private int tjJumpCount; // The amount of jumps built up for a triple jump so far.
    private float curAirBoostChargeTime;
    private float curAirBoostTime;
    private bool isAirBoosting;
    private bool isAirBoostCharging;
    private bool hasAirBoostedThisJump;
    private bool inAirBoostChargeAftermath;

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
    [HideInInspector] public UnityEvent mm_OnAirBoostChargeStart;
    [HideInInspector] public UnityEvent mm_OnAirBoostStart;
    [HideInInspector] public UnityEvent mm_OnAirBoostEnd;

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
        tjJumpCount = (tjTimeBtwnJumps > tjMaxTimeBtwnJumps) ? 1 : tjJumpCount + 1;
        tjCurJumpTime = 0;
        tjTimeBtwnJumps = 0;
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
        UpdateTripleJumpStatus();
        UpdateBoostStatus();
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
                hasAirBoostedThisJump = false;
                if (tjJumpCount == 3 || tjCurJumpTime > tjMaxJumpTime || tjCurJumpTime < tjMinJumpTime)
                    tjJumpCount = 0;
                tjCurJumpTime = 0;
                tjTimeBtwnJumps = 0;
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

    private void UpdateTripleJumpStatus()
    {
        if (isJumping)
        {
            tjCurJumpTime += Time.fixedDeltaTime;
        }
        else
        {
            tjTimeBtwnJumps += Time.fixedDeltaTime;
        }

        // Cancel
        if (tjJumpCount != 3 && (GetHorizDissonance() > tjMaxDissonance || GetHorizontalInput().magnitude < tjMinInputMagnitude))
        {
            tjJumpCount = 0;
        }
    }

    private void UpdateBoostStatus()
    {
        if (!isOnGround)
        {
            if (inputActions.Player.Boost.ReadValue<float>() > 0 && !isAirBoosting && !hasAirBoostedThisJump)
            {
                if (!isAirBoostCharging)
                {
                    // First frame, air boost charge
                    tjJumpCount = 0;
                    isAirBoostCharging = true;
                    mm_OnAirBoostChargeStart.Invoke();
                }

                // Charging air boost
                curAirBoostChargeTime += Time.deltaTime;

                if (curAirBoostChargeTime > airBoostMaxChargeTime)
                {
                    curAirBoostChargeTime = airBoostMaxChargeTime;
                }
            }

            if (curAirBoostChargeTime > 0f && inputActions.Player.Boost.ReadValue<float>() == 0)
            {
                // First frame of air boost
                hasAirBoostedThisJump = true;
                // Start mid-boost if low charge
                isAirBoostCharging = false;
                curAirBoostTime = airBoostMaxTime - (airBoostMaxTime * (curAirBoostChargeTime / airBoostMaxChargeTime));
                curAirBoostChargeTime = 0;
                isAirBoosting = true;
                mm_OnAirBoostStart.Invoke();
            }

            if (isAirBoosting)
            {
                // Doing air boost
                curAirBoostTime += Time.deltaTime;

                if (curAirBoostTime > airBoostMaxTime || isOnGround)
                {
                    // Last frame of air boost
                    inAirBoostChargeAftermath = true;
                    curAirBoostTime = 0;
                    isAirBoosting = false;
                    mm_OnAirBoostEnd.Invoke();
                }
            }

            if (inputActions.Player.Boost.ReadValue<float>() == 0 || isAirBoosting)
            {
                curAirBoostChargeTime = 0;
            }
        }

        if (isOnGround)
        {
            inAirBoostChargeAftermath = false;
            curAirBoostChargeTime = 0;
            curAirBoostTime = 0;
            isAirBoosting = false;
        }

        // TODO for air boosting
        // - Rotation restricted during it
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

    public bool InTripleJump()
    {
        return tjJumpCount == 3;
    }

    public bool InAirBoost()
    {
        return isAirBoosting;
    }

    public bool InAirBoostCharge()
    {
        return isAirBoostCharging;
    }

    public bool InAirBoostChargeAftermath()
    {
        return inAirBoostChargeAftermath;
    }

    public float getMaxChargeTime()
    {
        return airBoostMaxChargeTime;
    }
}
