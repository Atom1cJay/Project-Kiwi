using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    private bool colliding;
    private GameObject collidingWith = null;
    [SerializeField] private List<GameObject> gameObjectsToIgnore;
    [SerializeField] LayerMask layersToInclude;

    private void UpdateCollisionStatus()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.right * (transform.localScale.magnitude / 2) / 1.3f));
        foreach (Collider c in Physics.OverlapSphere(transform.position, (transform.localScale.magnitude / 2) / 1.3f, layersToInclude)) // TODO FIX BAD GENERALIZE
        {
            if (!c.isTrigger && !gameObjectsToIgnore.Contains(c.gameObject))
            {
                collidingWith = c.gameObject;
                colliding = true;
                return;
            }
        }

        collidingWith = null;
        colliding = false;
    }

    /// <summary>
    /// Is the Collision Detector colliding with something it can detect?
    /// </summary>
    /// <returns>Whether or not the <code>CollisionDetector</code> is activated</returns>
    public bool Colliding()
    {
        UpdateCollisionStatus();
        return colliding;
    }

    /// <summary>
    /// Returns the object this collider is currently colliding with. Returns
    /// null if there is no such object.
    /// </summary>
    /// <returns></returns>
    public GameObject CollidingWith()
    {
        UpdateCollisionStatus();
        return collidingWith;
    }
}
