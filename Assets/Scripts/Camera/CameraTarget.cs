using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the object which the camera is made to focus on / follow.
/// </summary>
public class CameraTarget : MonoBehaviour
{
    // Basic Needs
    [SerializeField] private Transform player;

    // For transitioning
    [SerializeField] Camera mainCam;
    [SerializeField] float sensitivity;
    [SerializeField] float minViewportY;
    [SerializeField] float maxViewportY;
    [SerializeField] float maxPosDiffY;
    [SerializeField] float minPosDiffY;

    private void Awake()
    {
        ResetToPlayerCenter();
    }

    /// <summary>
    /// Moves vertically by one frame according to the current position of the
    /// player relative to the target.
    /// </summary>
    public void Adjust()
    {
        AdjustToCamTarget();
    }

    /// <summary>
    /// Instantly places the camera target exactly at the player's position.
    /// </summary>
    public void ResetToPlayerCenter()
    {
        transform.position = player.position;
    }

    /// <summary>
    /// Executes the details of vertically moving the target by one frame
    /// according to the current position of the player relative to the target.
    /// </summary>
    private void AdjustToCamTarget()
    {
        float myYChange = 0;
        float yDiff = player.position.y - transform.position.y;

        if (yDiff > maxPosDiffY) // If player too high
        {
            myYChange = Mathf.Pow(yDiff, 2);
        }
        else if (yDiff < minPosDiffY) // If player too low
        {
            myYChange = -Mathf.Pow(yDiff, 2);
        }

        transform.position =
            new Vector3(
                player.position.x,
                transform.position.y + (myYChange * sensitivity * Time.deltaTime),
                player.position.z);
    }
}
