using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointLoader : MonoBehaviour
{
    [SerializeField] InputActionsHolder IAH;
    CheckpointSystem currentCheckpoint = null;

    /// <summary>
    /// Gives the current checkpoint activated by the player (null if none are).
    /// </summary>
    /// <returns></returns>
    public CheckpointSystem GetCheckpoint()
    {
        return currentCheckpoint;
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
