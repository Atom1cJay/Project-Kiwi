using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementMaster))]
[System.Serializable]
public class VerticalMovement : MonoBehaviour
{
    [Header("Regular Jump")]
    [SerializeField] private float initJumpVel;
    [SerializeField] private float initGravity;
    [SerializeField] private float maxGravity;
    [SerializeField] private float maxGravityAtCancel;
    [SerializeField] private float velocityMultiplierAtCancel = 0.5f;
    [SerializeField] private float gravityIncRate;
    [SerializeField] private float gravityIncRateAtCancel;
    [Header("Triple Jump")]
    [SerializeField] private float tjInitJumpVel;
    [SerializeField] private float tjInitGravity;
    [SerializeField] private float tjMaxGravity;
    [SerializeField] private float tjMaxGravityAtCancel;
    [SerializeField] private float tjVelocityMultiplierAtCancel = 0.5f;
    [SerializeField] private float tjGravityIncRate;
    [SerializeField] private float tjGravityIncRateAtCancel;
    [Header("Non-Jumping")]
    [SerializeField] private float nonJumpGravity;
    [SerializeField] private float airBoostGravity;
    [SerializeField] private float airBoostChargeGravity;
    [SerializeField] private float airBoostEndGravity;
    [SerializeField] private float minVertAirBoostVel;
    [SerializeField] private float maxVertAirBoostVel;
    [SerializeField] private float vertAirBoostGravity;
    [SerializeField] private float airDivingVel;
    private float gravity;
    private float vertVel;
    private float frameVerticalMovement;
    private MovementMaster mm;

    Vector3 amountToMove;

    private void Awake()
    {
        mm = GetComponent<MovementMaster>();
        mm.mm_OnJump.AddListener(OnJump);
        mm.mm_OnJumpCanceled.AddListener(OnJumpCanceled);
        mm.mm_OnFirstFrameGrounded.AddListener(OnFirstFrameGrounded);
        mm.mm_FixedUpdateWhileGrounded.AddListener(FixedUpdateWhileOnGround);
        mm.mm_OnAirBoostStart.AddListener(OnAirBoostStart);
        mm.mm_OnAirBoostEnd.AddListener(OnAirBoostEnd);
        mm.mm_OnAirBoostChargeStart.AddListener(OnAirBoostChargeStart);
        mm.mm_OnVertAirBoostChargeStart.AddListener(OnVertAirBoostCharge);
        mm.mm_OnVertAirBoostStart.AddListener(OnVertAirBoost);
        mm.mm_OnAirDiveStart.AddListener(OnAirDiveStart);
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
        if (mm.InTripleJump())
        {
            StopCoroutine("Jump");
            StopCoroutine("TripleJump");
            StartCoroutine("TripleJump");
        }
        else
        {
            StopCoroutine("Jump");
            StopCoroutine("TripleJump");
            StartCoroutine("Jump");
        }
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
            if (mm.InTripleJump())
            {
                vertVel *= tjVelocityMultiplierAtCancel;
            }
            else
            {
                vertVel *= velocityMultiplierAtCancel;
            }
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
                gravity += gravityIncRateAtCancel * Time.fixedDeltaTime;
            else
                gravity += gravityIncRate * Time.fixedDeltaTime;

            if (gravity > maxGravity && !mm.JumpInputCancelled())
                gravity = maxGravity;
            else if (gravity > maxGravityAtCancel && mm.JumpInputCancelled())
                gravity = maxGravityAtCancel;

            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Manipulates the gravity (and, initially, the vertical velocity) in order
    /// to simulate a triple jump. If the jump is cancelled, modifies the arc
    /// the player is on.
    /// </summary>
    private IEnumerator TripleJump()
    {
        gravity = tjInitGravity;
        vertVel = tjInitJumpVel;

        while (mm.IsJumping())
        {
            if (mm.JumpInputCancelled())
                gravity += tjGravityIncRateAtCancel * Time.fixedDeltaTime;
            else
                gravity += tjGravityIncRate * Time.fixedDeltaTime;

            if (gravity > tjMaxGravity && !mm.JumpInputCancelled())
                gravity = tjMaxGravity;
            else if (gravity > tjMaxGravityAtCancel && mm.JumpInputCancelled())
                gravity = tjMaxGravityAtCancel;

            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Handle final movement for each frame
    /// </summary>
    private void FixedUpdate()
    {
        amountToMove = Vector3.zero;

        if (!mm.InAirBoost() && !mm.InAirBoostCharge() && !mm.InVertAirBoostCharge() && !mm.IsAirDiving())
        {
            EnforceGravity();
        }

        MoveByVertVel();
    }

    private void Update()
    {
        frameVerticalMovement = amountToMove.y;
        mm.GetCharacterController().Move(amountToMove * Time.deltaTime);
    }

    /// <summary>
    /// Do stuff for the first frame the ground is touched
    /// </summary>
    private void OnFirstFrameGrounded()
    {
        // Ensure player makes direct contact ground upon hitting it
        amountToMove += (new Vector3(0, -1, 0));
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
            vertVel -= gravity * Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// Moves the player by the current vertical velocity
    /// </summary>
    private void MoveByVertVel()
    {
        amountToMove += (new Vector3(0, vertVel, 0));
    }

    private void OnAirBoostChargeStart()
    {
        StartCoroutine("HandleVelDuringAirBoostCharge");
    }

    private void OnAirBoostStart()
    {
        vertVel = 0;
        StartCoroutine("HandleVelDuringAirBoost");
    }

    IEnumerator HandleVelDuringAirBoost()
    {
        while (mm.InAirBoost())
        {
            vertVel -= airBoostGravity * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator HandleVelDuringAirBoostCharge()
    {
        if (vertVel < 0)
        {
            vertVel = 0;
        }

        while (mm.InAirBoostCharge())
        {
            if (vertVel > 0)
            {
                vertVel -= nonJumpGravity * Time.fixedDeltaTime;
            }
            else
            {
                vertVel -= airBoostChargeGravity * Time.fixedDeltaTime;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnAirBoostEnd()
    {
        gravity = nonJumpGravity;
    }

    private void OnAirDiveStart()
    {
        vertVel = airDivingVel;
    }

    private void OnVertAirBoostCharge()
    {
        StartCoroutine("HandleVelDuringVertAirBoostCharge");
    }

    IEnumerator HandleVelDuringVertAirBoostCharge()
    {
        if (vertVel < 0)
        {
            vertVel = 0;
        }

        while (mm.InVertAirBoostCharge())
        {
            if (vertVel > 0)
            {
                vertVel -= nonJumpGravity * Time.fixedDeltaTime;
            }
            else
            {
                vertVel -= airBoostChargeGravity * Time.fixedDeltaTime;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnVertAirBoost(float proportionCharged)
    {
        vertVel = minVertAirBoostVel + (proportionCharged * (maxVertAirBoostVel - minVertAirBoostVel));
        gravity = vertAirBoostGravity;
    }

    public float GetFrameVerticalMovement()
    {
        return frameVerticalMovement;
    }
}
