using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSave : MonoBehaviour
{
    public static CheckpointSave Instance;

    static int numCheckpoint = -1;


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

        //Debug.Log(numCheckpoint);
        //DontDestroyOnLoad(this);
    }

    public int getNum()
    {
        return numCheckpoint;
    }



    public void SetCheckpoint(CheckpointSystem _cs, CheckpointSystem[] checkpoints)
    {
        numCheckpoint = System.Array.IndexOf(checkpoints, _cs);
        //Debug.Log(numCheckpoint);
    }
}
