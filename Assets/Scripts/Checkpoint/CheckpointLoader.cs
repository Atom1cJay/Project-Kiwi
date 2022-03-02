using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointLoader : MonoBehaviour
{
    public static CheckpointLoader Instance;
    [SerializeField] InputActionsHolder IAH;
    [SerializeField] CheckpointSystem[] checkpoints;
    [SerializeField] CheckpointSystem firstCheckpoint;

    CheckpointSystem currentCheckpoint = null;
   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (this != Instance)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (CheckpointSave.Instance.getNum() == -1)
            SetCheckpoint(firstCheckpoint);
        else
            SetCheckpoint(checkpoints[CheckpointSave.Instance.getNum()]);
    }
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

        CheckpointSave.Instance.SetCheckpoint(cs, checkpoints);
    }
}
