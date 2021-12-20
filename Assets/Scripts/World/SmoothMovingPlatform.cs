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
    Vector3 mvmtThisFrame = Vector3.zero;
    Vector3 rotThisFrame = Vector3.zero;
    public UnityEvent onMove;
    float timeSpent;

    private void Awake()
    {
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

        rotThisFrame =
            new Vector3(
                rotSpeed.x,
                rotSpeed.y,
                rotSpeed.z) * Time.fixedDeltaTime;

        transform.Translate(mvmtThisFrame, Space.World);
        transform.Rotate(rotThisFrame, Space.World);
        onMove.Invoke();
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

    /// <summary>
    /// Given the position of some object "childed" to this moving
    /// platform, what should the CHANGE in the object's position be, specifically
    /// from rotation, after this frame?
    /// </summary>
    public Vector3 posChangeFromRotationThisFrame(Vector3 origPos)
    {
        Vector3 origRelativePos = new Vector3(origPos.x - transform.position.x, origPos.y - transform.position.y, origPos.z - transform.position.z);
        Vector3 newRelativePos = Quaternion.Euler(rotThisFrame.x, rotThisFrame.y, rotThisFrame.z) * origRelativePos;
        return newRelativePos - origRelativePos;
    }
}
