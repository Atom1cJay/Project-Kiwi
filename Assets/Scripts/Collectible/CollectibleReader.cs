using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CollectibleReader : MonoBehaviour
{
    public Collectible collectible;

    public bool tempForceNotCollected;

    [SerializeField] static bool collected;

    Collider collectibleCollider;

    MeshRenderer collectibleRenderer;

    MeshFilter meshFilter;


    // Start is called before the first frame update
    void Start()
    {
        //initialize the variables
        collectibleCollider = GetComponent<Collider>();
        collectibleRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        if (tempForceNotCollected)
            collected = false;

        meshFilter.mesh = collectible.GetMesh();

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
