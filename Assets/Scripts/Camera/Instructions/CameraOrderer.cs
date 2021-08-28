using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a list of instructions which can be sent to a camera on command.
/// </summary>
public class CameraOrderer : MonoBehaviour
{
    [SerializeField] ACameraInstruction instructions;
    [SerializeField] bool orderOnStart;
    [SerializeField] CameraControl cameraController;

    private void Start()
    {
        if (orderOnStart)
        {
            SendOrders();
        }
    }

    public void SendOrders()
    {
        cameraController.TakeInstructions(instructions);
    }
}
