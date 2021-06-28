using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class MovementMaster : MonoBehaviour
{
    // Serialized Fields
    [SerializeField] InputActionsHolder iah;
    MovementSettingsSO movementSettings;
    [SerializeField] public GameObject relevantCamera;
    [SerializeField] public CollisionDetector groundDetector;

    // Other variables for internal use only
    private bool isJumping;
    private bool groundable; // Should a jump/vert boost end if player is touching ground?
    private bool inCoyoteTime;
    private bool isOnGround;
    private bool isInHardTurn;
    private float tjCurJumpTime; // The elapsed time for the current jump. 0 if there is no jump happening.
    private float tjTimeBtwnJumps;
    private int tjJumpCount; // The amount of jumps built up for a triple jump so far.
    private bool isAirBoosting;
    private bool isAirBoostCharging;
    private bool hasAirBoostedThisJump; // Applies to all boosts
    private bool isVertAirBoostCharging;
    private bool isGroundBoosting;
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

    // INITIALIZATION /////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Start()
    {
        InitializeAssets();
        InitializeInputEvents();
    }

    private void InitializeAssets()
    {
        movementSettings = MovementSettingsSO.Instance;
        mi = GetComponent<MovementInfo>();
        charCont = GetComponent<CharacterController>();
    }

    private void InitializeInputEvents()
    {
        iah.inputActions.Player.VertBoost.started += _ => OnVertBoostChargePerformed();
    }

    // STATE CONTROL /////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Jump()
    {
        tjJumpCount = (tjTimeBtwnJumps > movementSettings.TjMaxTimeBtwnJumps) ? 1 : tjJumpCount + 1;
        tjCurJumpTime = 0;
        tjTimeBtwnJumps = 0;
        isJumping = true;
        groundable = false;
        Invoke("MakeGroundable", movementSettings.JumpGroundableTimer);
        mm_OnJump.Invoke();
    }

    IEnumerator ReverseCoyoteTime()
    {
        float timePassed = 0f;

        while (timePassed < movementSettings.ReverseCoyoteTime)
        {
            bool pressingJump = iah.inputActions.Player.Jump.ReadValue<float>() > 0;

            if (isOnGround && pressingJump)
            {
                Jump();
                timePassed = movementSettings.ReverseCoyoteTime;
            }

            if (!pressingJump)
            {
                timePassed = movementSettings.ReverseCoyoteTime;
            }

            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void MakeGroundable()
    {
        groundable = true;
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
                if (tjJumpCount == 3 || tjCurJumpTime > movementSettings.TjMaxJumpTime || tjCurJumpTime < movementSettings.TjMinJumpTime)
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

        while (timePassed < movementSettings.CoyoteTime && !isJumping)
        {
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        inCoyoteTime = false;
    }

    private void UpdateHorizontalStates()
    {
        if (GetHorizDissonance() > movementSettings.DissonanceForHardTurn && mi.currentSpeedHoriz > movementSettings.HardTurnMinSpeed && !isInHardTurn && isOnGround)
        {
            // First frame of hard turn
            StartHardTurn();
        }

        if (!isOnGround && GetHorizDissonance() > movementSettings.DissonanceForAirReverse /*&& horizMove.GetSpeed() > movementSettings.AirReverseMinActivationSpeed*/)
        {
            isAirReversing = true; // True until hitting ground
        }
    }

    private void StartHardTurn()
    {
        isInHardTurn = true;
        mm_OnHardTurnStart.Invoke();
        Invoke("EndHardTurn", movementSettings.HardTurnTime);
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
        if (tjJumpCount != 3 && (GetHorizDissonance() > movementSettings.TjMaxDissonance || GetHorizontalInput().magnitude < movementSettings.TjMinHorizInputMagnitude))
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

    private void OnVertBoostChargePerformed()
    {
        if (isOnGround || isAirBoosting || isAirBoostCharging || isVertAirBoostCharging || hasAirBoostedThisJump /*|| isAirDiving*/)
            return;

        tjJumpCount = 0;
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

    public bool InAirBoost()
    {
        return isAirBoosting;
    }

    public bool InAirBoostCharge()
    {
        return isAirBoostCharging;
    }

    public bool InVertAirBoostCharge()
    {
        return isVertAirBoostCharging;
    }

    public bool IsGroundBoosting()
    {
        return isGroundBoosting;
    }

    public bool IsAirReversing()
    {
        return isAirReversing;
    }

    public bool tripleJumpValid()
    {
        return tjJumpCount == 3;
    }
}
