using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Collider))]
public class CollectibleReader : MonoBehaviour
{
    public Collectible collectible;

    public bool tempForceNotCollected;

    Collider collectibleCollider;

    public bool collected;

    public string name;

    [SerializeField] GameObject collectibleVisual;

    // Start is called before the first frame supdate
    void Start()
    {
        collected = CollectibleSystem.collected.Contains(gameObject.transform.position);

        //initialize the variables
        collectibleCollider = GetComponent<Collider>();

        if (tempForceNotCollected)
            collected = false;

        collectibleCollider.enabled = !collected;
        collectibleVisual.SetActive(!collected);

    }

    //Update the Collectible Reader
    public void UpdateCollectibleReader()
    {
        collectibleCollider.enabled = !collected;
        collectibleVisual.SetActive(!collected);
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
