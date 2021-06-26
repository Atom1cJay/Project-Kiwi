using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private GameObject collidingWith = null;
    [SerializeField] private List<GameObject> gameObjectsToIgnore;

    /// <summary>
    /// Is the Collision Detector colliding with something it can detect?
    /// </summary>
    /// <returns>Whether or not the <code>CollisionDetector</code> is activated</returns>
    public bool Colliding()
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, (transform.localScale.magnitude / 2) / 1.3f)) // TODO FIX BAD GENERALIZE
        {
            if (!gameObjectsToIgnore.Contains(c.gameObject))
            {
                collidingWith = c.gameObject;
                return true;
            }
        }

        collidingWith = null;
        return false;
    }

    /// <summary>
    /// Returns the object this collider is currently colliding with. Returns
    /// null if there is no such object.
    /// </summary>
    /// <returns></returns>
    public GameObject CollidingWith()
    {
        return collidingWith;
    }
}
