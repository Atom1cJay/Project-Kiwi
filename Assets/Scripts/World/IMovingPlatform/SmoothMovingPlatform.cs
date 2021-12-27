using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// For a moving platform which can rotate at a constant speed
/// and move in the pattern of a sine wave. Moves in the Update() method instead
/// of the FixedUpdate() method.
/// </summary>
public class SmoothMovingPlatform : AMovingPlatform
{
    // What type of space does the movement vector take place in?
    public enum MovementSpace
    {
        World, // Relative to world
        Relative, // Relative to the rotation of relativeTransform
        RelativeToInitial // Relative to the INITIAL rotation of relativeTransform
    }

    [SerializeField] Vector3 distanceToMove;
    [SerializeField] float movementSpeed;
    [SerializeField] Vector3 rotSpeed;
    [SerializeField] float timeOffset;
    [SerializeField] MovementSpace movementSpace;
    [SerializeField] bool rotationIsRelative;
    [SerializeField] Transform relativeTransform; // Relative movement / rotation is relative to this (default: this.transform)
    [SerializeField] GameObject empty; // To be spawned as child when player is on this platform
    float timeSpent;
    float waveValue;
    Vector3 midpoint;
    Vector3 mvmtThisFrame = Vector3.zero;
    Vector3 rotThisFrame = Vector3.zero;
    Vector3 initTransformRight;
    Vector3 initTransformUp;
    Vector3 initTransformForward;
    GameObject phantom; // Child object, to have its rotation tracked so we know how to rotate the player

    private void Awake()
    {
        if (relativeTransform == null)
        {
            relativeTransform = transform;
        }
        // Set up initial values
        midpoint = transform.position + (GetDistanceToMove() / 2);
        initTransformRight = transform.right;
        initTransformForward = transform.forward;
        initTransformUp = transform.up;
    }

    private void OnEnable()
    {
        // Handle initial movement for time offset
        transform.position = midpoint + (-Mathf.Cos(movementSpeed * Mathf.PI * (Time.time + timeOffset)) * GetDistanceToMove() / 2);
        Rotate(rotSpeed * (Time.time + timeOffset));
    }

    /// <summary>
    /// Move the platform according to the current time passed in game.
    /// </summary>
    void Update()
    {
        timeSpent += Time.deltaTime;
        waveValue = movementSpeed * (Mathf.Sin(movementSpeed * Mathf.PI * (timeSpent + timeOffset)) * Mathf.PI / 2);
        HandleMovementThisFrame();
        HandleRotationThisFrame();
        base.onTransformChange.Invoke();
    }

    /// <summary>
    /// Move the platform for this frame, and store mvmtThisFrame.
    /// </summary>
    void HandleMovementThisFrame()
    {
        mvmtThisFrame = waveValue * GetDistanceToMove() * Time.deltaTime;
        transform.Translate(mvmtThisFrame, Space.World);
    }

    /// <summary>
    /// Rotate the platform for this frame, and store rotThisFrame.
    /// </summary>
    void HandleRotationThisFrame()
    {
        Rotate(rotSpeed * Time.deltaTime);
        rotThisFrame = rotSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Rotates by the given amount, either independently or around something
    /// depending on whether this object's rotation is relative.
    /// </summary>
    void Rotate(Vector3 amt)
    {
        if (rotationIsRelative)
        {
            float[] amtValues = { Mathf.Abs(amt.x), Mathf.Abs(amt.y), Mathf.Abs(amt.z) };
            transform.RotateAround(relativeTransform.position, amt / Mathf.Max(amtValues), Mathf.Max(amtValues));
        }
        else
        {
            transform.Rotate(amt, Space.World);
        }
    }

    /// <summary>
    /// Gives distanceToMove as an absolute vector, but if movement is relative,
    /// it takes into consideration the current angle of this object.
    /// </summary>
    Vector3 GetDistanceToMove()
    {
        switch (movementSpace)
        {
            case MovementSpace.World:
                return distanceToMove;
            case MovementSpace.Relative:
                return relativeTransform.right * distanceToMove.x
                    + relativeTransform.up * distanceToMove.y
                    + relativeTransform.forward * distanceToMove.z;
            case MovementSpace.RelativeToInitial:
                return initTransformRight * distanceToMove.x
                    + initTransformUp * distanceToMove.y
                    + initTransformForward * distanceToMove.z;
            default:
                return Vector3.zero;
        }
    }

    public override Vector3 PosChangeFromRotThisFrame(Vector3 origPos)
    {
        Rotate(-rotThisFrame);
        phantom.transform.position = origPos;
        Rotate(rotThisFrame);
        return phantom.transform.position - origPos;
    }

    public override Vector3 MvmtThisFrame()
    {
        return mvmtThisFrame;
    }

    public override Vector3 RotThisFrame()
    {
        return rotThisFrame;
    }

    public override void Register()
    {
        phantom = Instantiate(empty, transform);
    }

    public override void Deregister()
    {
        Destroy(phantom.gameObject);
        phantom = null;
    }
}
