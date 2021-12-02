using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToGround : MonoBehaviour
{
    [SerializeField] LayerMask validGround;
    [SerializeField] float offsetAboveRaycastHitPoint; // Distance above ground to float
    [SerializeField] float raycastLength;

    // Based on the given position, stick to the nearest "floor" below that position.
    public void UpdatePosition(Vector3 pos)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit, raycastLength, validGround))
        {
            transform.position = new Vector3(pos.x, hit.point.y + offsetAboveRaycastHitPoint, pos.z);
        }
    }
}
