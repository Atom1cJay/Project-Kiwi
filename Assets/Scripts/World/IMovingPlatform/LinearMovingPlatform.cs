using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovingPlatform : AMovingPlatform
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] Vector3 rotSpeed;
    [SerializeField] float timeOffset;
    [SerializeField] bool rotationIsRelative;
    [SerializeField] Transform relativeTransform; // Relative movement / rotation is relative to this (default: this.transform)
    [SerializeField] GameObject empty; // To be spawned as child when player is on this platform
    Vector3 mvmtThisFrame = Vector3.zero;
    Vector3 rotThisFrame = Vector3.zero;
    GameObject phantom; // Child object, to have its rotation tracked so we know how to rotate the player

    private void Awake()
    {
        if (relativeTransform == null)
        {
            relativeTransform = transform;
        }
    }

    private void OnEnable()
    {
        // Handle initial movement for time offset
        Rotate(rotSpeed * (Time.time + timeOffset));
    }

    /// <summary>
    /// Move the platform according to the current time passed in game.
    /// </summary>
    void Update()
    {
        HandleMovementThisFrame();
        HandleRotationThisFrame();
        base.onTransformChange.Invoke();
    }

    /// <summary>
    /// Move the platform for this frame, and store mvmtThisFrame.
    /// </summary>
    void HandleMovementThisFrame()
    {
        transform.Translate(movementVector * Time.deltaTime, Space.World);
        mvmtThisFrame = movementVector * Time.deltaTime;
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
