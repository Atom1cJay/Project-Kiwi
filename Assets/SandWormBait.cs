using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormBait : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("poggers");
        SandWormFSM sandWorm = other.GetComponentInParent<SandWormFSM>();

        if (sandWorm != null)
            Destroy(gameObject);
    }
}
