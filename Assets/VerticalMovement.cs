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
    [SerializeField] private float airBoostChargeVel;
    [SerializeField] private float airBoostGravity;
    [SerializeField] private float airBoostEndGravity;
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
        mm.mm_OnAirBoostStart.AddListener(OnAirBoostStart);
        mm.mm_OnAirBoostEnd.AddListener(OnAirBoostEnd);
        mm.mm_OnAirBoostChargeStart.AddListener(OnAirBoostChargeStart);
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
                gravity += tjGravityIncRateAtCancel * Time.deltaTime;
            else
                gravity += tjGravityIncRate * Time.deltaTime;

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
    private void Update()
    {
        if (!mm.InAirBoost() && !mm.InAirBoostCharge())
        {
            EnforceGravity();
        }

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

    private void OnAirBoostChargeStart()
    {
        vertVel = Mathf.Sign(vertVel) * airBoostChargeVel;
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
            vertVel -= airBoostGravity * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnAirBoostEnd()
    {
        gravity = airBoostGravity;
    }
}
