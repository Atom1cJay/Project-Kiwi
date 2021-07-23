using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the object which the camera is made to focus on
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

    private void Start()
    {
        transform.position = player.position;
    }

    public void Adjust()
    {
        AdjustToCamTarget();
        print("AdjustTarget");
    }

    private void AdjustToCamTarget()
    {
        float playerYPoint = mainCam.WorldToViewportPoint(player.position).y;

        float myYChange = 0;

        if (playerYPoint > maxViewportY)
        {
            myYChange = Mathf.Pow(playerYPoint - maxViewportY, 2);
        }
        else if (playerYPoint < minViewportY)
        {
            myYChange = -Mathf.Pow(playerYPoint - minViewportY, 2);
        }

        transform.position =
            new Vector3(
                player.position.x,
                transform.position.y + (myYChange * sensitivity * Time.deltaTime),
                player.position.z);
    }
}
