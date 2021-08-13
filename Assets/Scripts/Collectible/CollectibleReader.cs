using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectibleReader : MonoBehaviour
{
    public Collectible collectible;

    public bool tempForceNotCollected;

    [SerializeField] static bool collected;

    Collider collectibleCollider;

    MeshRenderer collectibleRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //initialize the variables
        collectibleCollider = GetComponent<Collider>();

        if (tempForceNotCollected)
            collected = false;

        collectibleCollider.enabled = !collected;
        collectibleRenderer.enabled = !collected;
    }

    //Update the Collectible Reader
    public void UpdateCollectibleReader()
    {
        collectibleCollider.enabled = !collected;
        collectibleRenderer.enabled = !collected;
    }

    //Returns the collectible
    public Collectible GetCollectible()
    {
        return collectible;
    }

    //Sets collected to true
    public void CollectObject()
    {
        collected = true;
        UpdateCollectibleReader();
    }


    //returns whether or not this collectible has been collected
    public bool IsCollected()
    {
        return collected;
    }
}
