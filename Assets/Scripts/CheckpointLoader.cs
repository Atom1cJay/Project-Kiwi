using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointLoader : MonoBehaviour
{
    [SerializeField] InputActionsHolder IAH;
    CheckpointSystem currentCheckpoint = null;

    void FixedUpdate()
    {
        //if button is pressed set transform
        if(IAH.inputActions.Checkpoint.Respawn.ReadValue<float>() > 0f && currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.GetPosition() + (Vector3.up * 3f);
            Debug.Log(currentCheckpoint.GetPosition() + Vector3.up);
        }
    }

    public void SetCheckpoint(CheckpointSystem cs)
    {
        if (currentCheckpoint != null)
        {
            currentCheckpoint.SetInactive();
        }
        currentCheckpoint = cs;
        currentCheckpoint.SetActive();
    }
}
