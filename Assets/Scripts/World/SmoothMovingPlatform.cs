using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// (EXPERIMENTAL) For a moving platform which can rotate at a constant speed
/// and move in the pattern of a sine wave. Moves in the Update() method instead
/// of the FixedUpdate() method.
/// </summary>
public class SmoothMovingPlatform : MonoBehaviour
{
    [SerializeField] Vector3 distanceToMove;
    [SerializeField] float speed;
    [SerializeField] Vector3 rotSpeed;
    [SerializeField] float timeOffset;
    [SerializeField] bool movementIsRelative;
    [SerializeField] Transform relativeTransform; // Relative movement is relative to the rotation
    float waveValue;
    Vector3 midpoint;
    Vector3 initRot;
    Vector3 mvmtThisFrame;
    public UnityEvent onMove;
    float timeSpent;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        midpoint = transform.position + (GetDistanceToMove() / 2);
        initRot = transform.rotation.eulerAngles;
    }

    private void OnEnable()
    {
        if (relativeTransform == null)
        {
            relativeTransform = transform;
        }
        transform.rotation = Quaternion.Euler(initRot);
        transform.Rotate(rotSpeed * (Time.time + timeOffset));
        transform.position = midpoint + (-Mathf.Cos(speed * (Time.time + timeOffset)) * GetDistanceToMove() / 2);
    }

    /// <summary>
    /// Move the platform according to the current time passed in game.
    /// </summary>
    void Update()
    {
        timeSpent += Time.deltaTime;
        waveValue = speed * (Mathf.Sin(speed * (timeSpent + timeOffset)) / 2);
    }

    private void FixedUpdate()
    {
        mvmtThisFrame =
            new Vector3(
                waveValue * GetDistanceToMove().x,
                waveValue * GetDistanceToMove().y,
                waveValue * GetDistanceToMove().z) * Time.fixedDeltaTime;

        Vector3 rotThisFrame =
            new Vector3(
                rotSpeed.x,
                rotSpeed.y,
                rotSpeed.z) * Time.fixedDeltaTime;

        transform.Translate(mvmtThisFrame, Space.World);
        //transform.position += mvmtThisFrame;
        //print(mvmtThisFrame);
        onMove.Invoke();
        transform.Rotate(rotThisFrame);
    }

    /// <summary>
    /// Approximates the movement the platform will make next frame.
    /// </summary>
    public Vector3 GetMovementNextFrame()
    {
        float timeSpentApproximated = timeSpent + Time.deltaTime;
        float waveValueApproximated = speed * (Mathf.Sin(speed * (timeSpentApproximated + timeOffset)) / 2);
        return new Vector3(
            waveValueApproximated * GetDistanceToMove().x,
            waveValueApproximated * GetDistanceToMove().y,
            waveValueApproximated * GetDistanceToMove().z) * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Gives distanceToMove as an absolute vector, but if movement is relative,
    /// it takes into consideration the current angle of this object.
    /// </summary>
    Vector3 GetDistanceToMove()
    {
        if (!movementIsRelative)
        {
            return distanceToMove;
        }
        return relativeTransform.right * distanceToMove.x
            + relativeTransform.up * distanceToMove.y
            + relativeTransform.forward * distanceToMove.z;
    }

    public Vector3 MvmtThisFrame()
    {
        return mvmtThisFrame;
    }
}
