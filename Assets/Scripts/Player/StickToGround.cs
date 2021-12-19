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

    // Based on the given position, stick to the nearest "floor" below that position.
    // Then, offset this object's position by the offset variable.
    public void UpdatePosition(Vector3 pos, Vector3 offset)
    {
        UpdatePosition(pos);
        transform.position += offset;
    }
}
