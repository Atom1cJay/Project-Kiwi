using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovingPlatform : AMovingPlatform
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
    float timeSpent;
    float waveValue;
    Vector3 midpoint;
    Vector3 initTransformRight;
    Vector3 initTransformUp;
    Vector3 initTransformForward;

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

    /// <summary>
    /// Rotates by the given amount, either independently or around something
    /// depending on whether this object's rotation is relative.
    /// </summary>
    public void RotateBy(Vector3 amt)
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

    private void OnEnable()
    {
        // Handle initial movement for time offset
        transform.position = midpoint + (-Mathf.Cos(movementSpeed * Mathf.PI * (Time.time + timeOffset)) * GetDistanceToMove() / 2);
        RotateBy(rotSpeed * (Time.time + timeOffset));
    }

    public override void FrameStart()
    {
        timeSpent += Time.deltaTime;
        waveValue = movementSpeed * (Mathf.Sin(movementSpeed * Mathf.PI * (timeSpent + timeOffset)) * Mathf.PI / 2);
    }

    public override void Translate()
    {
        Vector3 mvmtThisFrame = waveValue * GetDistanceToMove() * Time.deltaTime;
        transform.Translate(mvmtThisFrame, Space.World);
    }

    public override void Rotate()
    {
        RotateBy(rotSpeed * Time.deltaTime);
    }
}
