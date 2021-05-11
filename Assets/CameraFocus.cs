using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the object which the camera is made to focus on
/// </summary>
public class CameraFocus : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Start()
    {
        transform.position = player.position;
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }
}
