using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormBait : MonoBehaviour
{
    [SerializeField] bool killOnContact = true;

    private void OnTriggerEnter(Collider other)
    {
        SandWormFSM sandWorm = other.GetComponentInParent<SandWormFSM>();

        if (sandWorm != null)
            Destroy(gameObject);
    }
}
