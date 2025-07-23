using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDeactivator : MonoBehaviour
{
    [SerializeField] MonoBehaviour toDeactivate;
    [SerializeField] float deactivateDistance;

    private void Start()
    {
        DistanceDeactivatorHandler.instance.Register(this);
    }

    /// <summary>
    /// Based on the DistanceDeactivatorHandler's position, is this object so
    /// far away that it should deactivate?
    /// </summary>
    public void HandleDistance(Vector3 ddhPos)
    {
        if (toDeactivate != null)
        {
            if (toDeactivate.enabled && Vector3.Distance(ddhPos, transform.position) > deactivateDistance)
            {
                toDeactivate.enabled = false;
            }
            else if (!toDeactivate.enabled && Vector3.Distance(ddhPos, transform.position) < deactivateDistance)
            {
                toDeactivate.enabled = true;
            }
        }
    }
}
