using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    [SerializeField] Material currentCheckpointMat,inactiveCheckpointMat;
    [SerializeField] CheckpointLoader cL;
    [SerializeField] bool makeFirst;
    MeshRenderer mr;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        SetInactive();
        if(makeFirst)
            cL.SetCheckpoint(this);
    }

    //Set checkpoint to inactive
    public void SetInactive()
    {
        mr.material = inactiveCheckpointMat;
    }

    //Set checkpoint to active
    public void SetActive()
    {
        mr.material = currentCheckpointMat;
    }

    //get position of checkpoint
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 9)
        {
            cL.SetCheckpoint(this);
        }
    }
}
