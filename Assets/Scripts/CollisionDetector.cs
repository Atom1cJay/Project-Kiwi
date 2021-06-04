using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private GameObject collidingWith = null;
    [SerializeField] private List<GameObject> gameObjectsToIgnore;
    //private bool collidingThisFrame;

    /*
    private void OnTriggerExit(Collider other)
    {
        if (!gameObjectsToIgnore.Contains(other.gameObject))
        {
            print("collision exiting");
            collidingThisFrame = false;
            collidingWith = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameObjectsToIgnore.Contains(other.gameObject))
        {
            print("collision staying");
            collidingThisFrame = true;
            collidingWith = other.gameObject;
        }
    }
    */

    private void FixedUpdate()
    {
        
    }

    /// <summary>
    /// Is the Collision Detector colliding with something it can detect?
    /// </summary>
    /// <returns>Whether or not the <code>CollisionDetector</code> is activated</returns>
    public bool Colliding()
    {
        RaycastHit hit;

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

        /*
        for (int i = 0; i < 360; i += 36)
        {
            //Check if anything with the platform layer touches this object
            if (Physics.SphereCast(transform.position, transform.lossyScale.magnitude / 2, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit))
            {
                if (!gameObjectsToIgnore.Contains(hit.collider.gameObject))
                {
                    collidingWith = hit.collider.gameObject;
                    return true;
                }
            }
        }
        collidingWith = null;
        return false;
        */
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
