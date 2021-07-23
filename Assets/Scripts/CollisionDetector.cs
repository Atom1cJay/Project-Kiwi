using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    private bool colliding;
    private GameObject collidingWith = null;
    [SerializeField] private List<GameObject> gameObjectsToIgnore;
    [SerializeField] CollectibleSystem cs;

    private void UpdateCollisionStatus()
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, (transform.localScale.magnitude / 2) / 1.3f)) // TODO FIX BAD GENERALIZE
        {
            if (!gameObjectsToIgnore.Contains(c.gameObject))
            {
                collidingWith = c.gameObject;
                colliding = true;
                return;
            }
        }

        collidingWith = null;
        colliding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            //print("yeah");
        }
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
