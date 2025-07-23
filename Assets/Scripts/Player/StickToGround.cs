using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: THE NAME OF THIS SCRIPT IS OUTDATED!
// DO NOT PUT THIS SCRIPT ON ANYTHING OTHER THAN THE PLAYER DROP SHADOW
// CREATOR.
// THIS SCRIPT IS TAILORED TO WORK WITH THE DIRECTIONAL LIGHT SPECIFICALLY.
public class StickToGround : MonoBehaviour
{
    [SerializeField] LayerMask validGround;
    [SerializeField] float distFromGround; // Distance above ground to float
    [SerializeField] float raycastLength;
    [SerializeField] Transform sun;

    private void Awake()
    {
        if (sun == null)
        {
            sun = GameObject.FindGameObjectWithTag("Sun").transform;
        }
    }

    // Based on the given position, stick to the nearest "floor" below that position.
    public void UpdatePosition(Vector3 pos)
    {
        if (sun == null)
        {
            Debug.LogError("Cannot update position of drop shadow because sun is not provided.");
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit, raycastLength, validGround))
        {
            Vector3 goalShadowPos = hit.point;
            transform.position = goalShadowPos - (sun.transform.forward * distFromGround);
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
