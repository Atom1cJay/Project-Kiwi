using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector3 distanceToMove;
    [SerializeField] float speed;
    [SerializeField] Vector3 rotSpeed;
    [SerializeField] float timeOffset;
    [SerializeField] bool movementIsRelative;
    [SerializeField] Transform relativeTransform; // Relative movement is relative
    // to the rotation of this object
    Vector3 mvmtThisFrame;
    Vector3 rotThisFrame;

    private void Start()
    {
        if (relativeTransform == null)
        {
            relativeTransform = transform;
        }
        transform.Rotate(rotSpeed * timeOffset);
        transform.position += GetDistanceToMove() * ((-Mathf.Cos(speed * timeOffset) / 2) /*- (timeOffset / 4)*/);
    }

    void Update()
    {
        float waveValue = speed * (Mathf.Sin(speed * (Time.time + timeOffset)) / 2);

        mvmtThisFrame =
            new Vector3(
                waveValue * GetDistanceToMove().x,
                waveValue * GetDistanceToMove().y,
                waveValue * GetDistanceToMove().z)* Time.fixedDeltaTime;

        rotThisFrame =
            new Vector3(
                rotSpeed.x,
                rotSpeed.y,
                rotSpeed.z) * Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        transform.Translate(mvmtThisFrame);
        transform.Rotate(rotThisFrame);
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
}
