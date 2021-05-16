using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private bool isColliding;
    private GameObject collidingWith = null;
    [SerializeField] private List<GameObject> gameObjectsToIgnore;

    private void OnTriggerStay(Collider other)
    {
        if (!gameObjectsToIgnore.Contains(other.gameObject))
        {
            isColliding = true;
            collidingWith = other.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameObjectsToIgnore.Contains(other.gameObject))
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isColliding = false;
        collidingWith = null;
    }

    /// <summary>
    /// Is the Collision Detector colliding with something it can detect?
    /// </summary>
    /// <returns>Whether or not the <code>CollisionDetector</code> is activated</returns>
    public bool Colliding()
    {
        return isColliding;
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
