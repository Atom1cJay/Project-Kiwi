using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointLoader : MonoBehaviour
{
    [SerializeField] CheckpointSystem firstCheckpoint;
    [SerializeField] InputActionsHolder IAH;

    CheckpointSystem currentCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        currentCheckpoint = firstCheckpoint;
        currentCheckpoint.SetActive();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if button is pressed set transform
        if(IAH.inputActions.Checkpoint.Respawn.ReadValue<float>() > 0f)
        {
            transform.position = currentCheckpoint.GetPosition() + (Vector3.up * 3f);
            Debug.Log(currentCheckpoint.GetPosition() + Vector3.up);

        }
        
    }

    public void SetCheckpoint(CheckpointSystem cs)
    {
        currentCheckpoint.SetInactive();
        currentCheckpoint = cs;
        currentCheckpoint.SetActive();

    }
}
