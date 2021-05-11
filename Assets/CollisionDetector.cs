using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private bool isColliding;
    [SerializeField] private List<GameObject> gameObjectsToIgnore;

    private void OnTriggerStay(Collider other)
    {
        if (!gameObjectsToIgnore.Contains(other.gameObject))
        {
            isColliding = true;
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
    }

    /// <summary>
    /// Is the Collision Detector colliding with something it can detect?
    /// </summary>
    /// <returns>Whether or not the <code>CollisionDetector</code> is activated</returns>
    public bool Colliding()
    {
        return isColliding;
    }
}
