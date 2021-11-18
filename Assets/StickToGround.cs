using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToGround : MonoBehaviour
{
    [SerializeField] LayerMask validGround;
    [SerializeField] float offsetAboveRaycastHitPoint; // Distance above ground to float
    [SerializeField] float raycastLength;

    public void UpdateShadowCaster(Vector3 playerPos)
    {
        RaycastHit hit;
        if (Physics.Raycast(playerPos, Vector3.down, out hit, raycastLength, validGround))
        {
            print(hit.collider.gameObject);
            transform.position = new Vector3(playerPos.x, hit.point.y + offsetAboveRaycastHitPoint, playerPos.z);
        }
    }
}
