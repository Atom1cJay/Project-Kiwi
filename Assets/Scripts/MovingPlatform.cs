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
    Vector3 mvmtThisFrame;
    Vector3 rotThisFrame;

    private void Start()
    {
        transform.Rotate(rotSpeed * timeOffset);
        transform.position += (GetDistanceToMove() * (Mathf.Sin(speed * timeOffset) / 2));
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
        return transform.right * distanceToMove.x
            + transform.up * distanceToMove.y
            + transform.forward * distanceToMove.z;
    }
}
