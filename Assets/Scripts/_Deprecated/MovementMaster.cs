using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementMaster : MonoBehaviour
{
    // Serialized Fields
    [SerializeField] InputActionsHolder iah;
    [SerializeField] public GameObject relevantCamera;
    [SerializeField] public CollisionDetector groundDetector;
    [Header("Jumping Settings")]
    [SerializeField] public float groundableTimer = 0.1f;
    [Header("Coyote Time Settings")]
    [SerializeField] public float reverseCoyoteTime;
    [SerializeField] public float coyoteTime;
    [Header("Hard Turn Settings")]
    [SerializeField] public float dissonanceForHardTurn;
    [SerializeField] public float hardTurnMinSpeed;
    [SerializeField] public float hardTurnTime;
    [Header("Triple Jump Settings")]
    [SerializeField] public float tjMinInputMagnitude;
    [SerializeField] public float tjMaxTimeBtwnJumps;
    [SerializeField] public float tjMinJumpTime;
    [SerializeField] public float tjMaxJumpTime;
    [SerializeField] public float tjMaxDissonance;
    [Header("Boosting Settings")]
    [SerializeField] public float airBoostMaxChargeTime;
    [SerializeField] public float airBoostMaxTime;
    [SerializeField] public float vertAirBoostMaxChargeTime;
    [Header("Misc. Settings")]
    [SerializeField] public float dissonanceForAirReverse;
    [SerializeField] public float airReverseMinActivationSpeed;

    // Other variables for internal use only
    private bool isJumping;
    private bool groundable; // Should a jump/vert boost end if player is touching ground?
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
    private bool hasAirBoostedThisJump; // Applies to all boosts
    private bool inAirBoostAftermath;
    private bool isVertAirBoostCharging;
    private bool isGroundBoosting;
    private bool isAirDiving;
    private bool isAirReversing;

    // Helpful Assets for Subclasses
    private CharacterController charCont;
    private MovementInfo mi;

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
    [HideInInspector] public UnityEvent mm_OnVertAirBoostChargeStart;
    [HideInInspector] public UnityFloatEvent mm_OnVertAirBoostStart;
    [HideInInspector] public UnityEvent mm_OnAirDiveStart;
    [HideInInspector] public UnityEvent mm_OnDiveRecoveryStart;

    // INITIALIZATION /////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Awake()
    {
        InitializeAssets();
        InitializeInputEvents();
    }

    private void InitializeAssets()
    {
        mi = GetComponent<MovementInfo>();
        charCont = GetComponent<CharacterController>();
    }

    private void InitializeInputEvents()
    {
        iah.inputActions.Player.Jump.performed += _ => OnJumpInputPerformed();
        iah.inputActions.Player.Jump.canceled += _ => OnJumpInputCanceled();
        iah.inputActions.Player.Boost.started += _ => OnBoostPerformed();
        iah.inputActions.Player.VertBoost.started += _ => OnVertBoostChargePerformed();
        iah.inputActions.Player.Dive.performed += _ => OnDivePerformed();
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
        groundable = false;
        Invoke("MakeGroundable", groundableTimer);
        mm_OnJump.Invoke();
    }

    IEnumerator ReverseCoyoteTime()
    {
        float timePassed = 0f;

        while (timePassed < reverseCoyoteTime)
        {
            bool pressingJump = iah.inputActions.Player.Jump.ReadValue<float>() > 0;

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

    private void MakeGroundable()
    {
        groundable = true;
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
        UpdateParentingStatus();
    }

    private void UpdateVerticalStates()
    {
        bool touchingGround = groundDetector.Colliding();

        // Decide if is on ground
        if (touchingGround && (!isJumping || groundable))
        {
            // On ground
            
            if (!isOnGround /*&& vertMove.GetFrameVerticalMovement() <= 0.01f*/)
            {
                // First frame on ground
                isAirReversing = false;
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

        if (isOnGround && groundable)
        {
            isJumping = false;
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
        if (GetHorizDissonance() > dissonanceForHardTurn && mi.currentSpeedHoriz > hardTurnMinSpeed && !isInHardTurn && isOnGround)
        {
            // First frame of hard turn
            StartHardTurn();
        }

        if (!isOnGround && GetHorizDissonance() > dissonanceForAirReverse /*&& horizMove.GetSpeed() > airReverseMinActivationSpeed*/)
        {
            isAirReversing = true; // True until hitting ground
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

    private void UpdateParentingStatus()
    {
        if (isOnGround && !isJumping)
        {
            GameObject ground = groundDetector.CollidingWith();

            if (ground.CompareTag("Moving Platform (Has Wrapper)"))
            {
                transform.SetParent(groundDetector.CollidingWith().transform.parent, true);
            }
            else
            {
                transform.SetParent(null, true);
            }
        }
        else
        {
            transform.SetParent(null, true);
        }
    }

    private void OnBoostPerformed()
    {
        if (isAirDiving)
            return;

        if (isOnGround)
        {
            // First frame, ground boost
            isGroundBoosting = true;
            StartCoroutine("DoGroundBoost");
        }
        else
        {
            if (isAirBoosting || isAirBoostCharging || isVertAirBoostCharging || hasAirBoostedThisJump)
                return;

            // First frame, air boost charge
            isGroundBoosting = false;
            isJumping = false;
            tjJumpCount = 0;
            isAirBoostCharging = true;
            mm_OnAirBoostChargeStart.Invoke();
            hasAirBoostedThisJump = true;
            StartCoroutine("ChargeAirBoost");
        }
    }

    IEnumerator DoGroundBoost()
    {
        yield return new WaitUntil(() => iah.inputActions.Player.Boost.ReadValue<float>() == 0);
        isGroundBoosting = false;
    }

    IEnumerator ChargeAirBoost()
    {
        while (isAirBoostCharging && !isOnGround)
        {
            curAirBoostChargeTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();

            if (curAirBoostChargeTime > airBoostMaxChargeTime)
            {
                curAirBoostChargeTime = airBoostMaxChargeTime;
            }

            if (iah.inputActions.Player.Boost.ReadValue<float>() == 0 || curAirBoostChargeTime >= airBoostMaxChargeTime)
            {
                StartCoroutine("ExecuteAirBoost");
                isAirBoostCharging = false;
            }
        }

        // Cancel charge
        curAirBoostChargeTime = 0;
        isAirBoostCharging = false;
    }

    IEnumerator ExecuteAirBoost()
    {
        // First frame of air boost
        curAirBoostTime = airBoostMaxTime - (airBoostMaxTime * (curAirBoostChargeTime / airBoostMaxChargeTime));
        // ^ Start mid-boost if low charge
        curAirBoostChargeTime = 0;
        isAirBoosting = true;
        mm_OnAirBoostStart.Invoke();

        while (isAirBoosting && !isOnGround)
        {
            // Doing air boost
            curAirBoostTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();

            if (curAirBoostTime > airBoostMaxTime)
            {
                isAirBoosting = false;
            }
        }

        // Last frame of air boost
        curAirBoostTime = 0;
        isAirBoosting = false;
        mm_OnAirBoostEnd.Invoke();

        while (!isOnGround)
        {
            // Aftermath
            inAirBoostAftermath = true;
            yield return new WaitForEndOfFrame();
        }

        inAirBoostAftermath = false;
    }

    private void OnVertBoostChargePerformed()
    {
        if (isOnGround || isAirBoosting || isAirBoostCharging || isVertAirBoostCharging || hasAirBoostedThisJump || isAirDiving)
            return;

        // First frame, vert air boost charge
        isGroundBoosting = false;
        isJumping = false;
        tjJumpCount = 0;
        isVertAirBoostCharging = true;
        hasAirBoostedThisJump = true;
        mm_OnVertAirBoostChargeStart.Invoke();
        StartCoroutine("ChargeVertAirBoost");
    }

    IEnumerator ChargeVertAirBoost()
    {
        while (isVertAirBoostCharging && !isOnGround)
        {
            curAirBoostChargeTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();

            if (curAirBoostChargeTime > vertAirBoostMaxChargeTime)
            {
                curAirBoostChargeTime = vertAirBoostMaxChargeTime;
            }

            if (iah.inputActions.Player.VertBoost.ReadValue<float>() == 0 || curAirBoostChargeTime >= vertAirBoostMaxChargeTime)
            {
                ExecuteVertAirBoost();
                isVertAirBoostCharging = false;
            }
        }

        // Cancel charge
        curAirBoostChargeTime = 0;
        isVertAirBoostCharging = false;
    }

    void ExecuteVertAirBoost()
    {
        // Only frame of vert air boost
        mm_OnVertAirBoostStart.Invoke(curAirBoostChargeTime / vertAirBoostMaxChargeTime);
        curAirBoostChargeTime = 0;
        curAirBoostTime = 0;
    }

    void OnDivePerformed()
    {
        if (!isOnGround && !isAirBoostCharging && !isVertAirBoostCharging)
        {
            tjJumpCount = 0;
            isJumping = false;
            StartCoroutine("DoDiveSequence");
        }
    }

    IEnumerator DoDiveSequence()
    {
        isAirDiving = true;
        mm_OnAirDiveStart.Invoke();
        yield return new WaitUntil(() => isOnGround);
        isAirDiving = false;
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

    /// <summary>
    /// Gives the normalized horizontal movement input.
    /// </summary>
    /// <returns></returns>
    
    public Vector2 GetHorizontalInput()
    {
        Vector2 rawInput = iah.inputActions.Player.Move.ReadValue<Vector2>();

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

    public bool IsInAirBoostAftermath()
    {
        return inAirBoostAftermath;
    }

    public bool InVertAirBoostCharge()
    {
        return isVertAirBoostCharging;
    }

    public bool IsAirDiving()
    {
        return isAirDiving;
    }

    public bool IsGroundBoosting()
    {
        return isGroundBoosting;
    }

    public bool IsAirReversing()
    {
        return isAirReversing;
    }

    public InputActions ia()
    {
        return iah.inputActions;
    }

    public bool tripleJumpValid()
    {
        return tjJumpCount == 3;
    }
}
