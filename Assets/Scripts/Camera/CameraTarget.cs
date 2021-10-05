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
    // -1 for any of the sensitivities means just stick
    [SerializeField] float sensitivityY;
    [SerializeField] float sensitivityX;
    [SerializeField] float sensitivityZ;
    [SerializeField] float maxPosDiffY;
    [SerializeField] float minPosDiffY;
    [SerializeField] float maxPosDiffX;
    [SerializeField] float minPosDiffX;
    [SerializeField] float maxPosDiffZ;
    [SerializeField] float minPosDiffZ;
    [SerializeField] float maxMovementEachDir;

    private void Awake()
    {
        ResetToPlayerCenter();
    }

    /// <summary>
    /// Instantly places the camera target exactly at the player's position.
    /// </summary>
    public void ResetToPlayerCenter()
    {
        transform.position = player.position;
    }

    /// <summary>
    /// Moves vertically by one frame according to the current position of the
    /// player relative to the target.
    /// </summary>
    public void Adjust()
    {
        Vector3 mainCamTransformFwdXZ = mainCam.transform.forward;
        mainCamTransformFwdXZ.y = 0;
        mainCamTransformFwdXZ = mainCamTransformFwdXZ.normalized;
        Vector3 mainCamTransformRightXZ = mainCam.transform.right;
        mainCamTransformRightXZ.y = 0;
        mainCamTransformRightXZ = mainCamTransformRightXZ.normalized;
        // How far is player ahead of camera target
        Vector3 distRawXZ = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z);
        // X and Z player aheadness relative to cam.transform.forward.xz
        float xDistanceRelative = Mathf.Cos(Vector3.Angle(distRawXZ, mainCamTransformRightXZ) * Mathf.Deg2Rad) * distRawXZ.magnitude;
        float zDistanceRelative = Mathf.Cos(Vector3.Angle(distRawXZ, mainCamTransformFwdXZ) * Mathf.Deg2Rad) * distRawXZ.magnitude;
        // How much should this pos change
        float yChange = AdjustToCamTarget(minPosDiffY, maxPosDiffY, player.position.y - transform.position.y);
        float xChange = AdjustToCamTarget(minPosDiffX, maxPosDiffX, xDistanceRelative);
        float zChange = AdjustToCamTarget(minPosDiffZ, maxPosDiffZ, zDistanceRelative);
        //yChange = Mathf.Clamp(yChange, -maxMovementEachDir, maxMovementEachDir);
        xChange = Mathf.Clamp(xChange, -maxMovementEachDir, maxMovementEachDir);
        zChange = Mathf.Clamp(zChange, -maxMovementEachDir, maxMovementEachDir);
        /*
        transform.position +=
            (zChange * sensitivityZ * mainCamTransformFwdXZ * Time.deltaTime) +
            (xChange * sensitivityX * mainCamTransformRightXZ * Time.deltaTime) +
            (yChange * sensitivityY * Vector3.up * Time.deltaTime);
        */
        transform.position +=
            new Vector3(
                player.position.x - transform.position.x,
                yChange * sensitivityY * Time.deltaTime,
                player.position.z - transform.position.z);
    }

    /// <summary>
    /// Gives the amount that a certain axis should change, depending on how far
    /// the player is ahead in that axis, and the minimum/maximum distance from
    /// this object that the player should be
    /// </summary>
    private float AdjustToCamTarget(float minDiff, float maxDiff, float diff)
    {
        float axisChange = 0;

        if (diff > maxDiff) // If player too high
        {
            axisChange = Mathf.Pow(diff - maxDiff, 2);
        }
        else if (diff < minDiff) // If player too low
        {
            axisChange = -Mathf.Pow(diff - minDiff, 2);
        }

        return axisChange;
    }
}
