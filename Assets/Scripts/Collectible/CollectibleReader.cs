using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Events;

//[RequireComponent(typeof(Collider))]
public class CollectibleReader : MonoBehaviour
{
    public Collectible collectible;

    public bool tempForceNotCollected;
    [SerializeField] UnityEvent onAutoCollectAtLoad; // If tempForceNotCollected is off, this will auto collect at the start if it's already been collected

    Collider collectibleCollider;

    public bool collected;

    public string name;

    [SerializeField] GameObject collectibleVisual;

    [SerializeField] bool playAnimation;
    [SerializeField] GameObject overheadVisual;

    Animator anim; // Optional! (Used if playAnimation is true)
    [SerializeField] float animLength; // Optional! (Used if playAnimation is true)

    public UnityEvent onCollect;

    public Vector3 startingPosIdentifier { get; private set; }
    bool initialized;

    // Effectively the Start() method.
    // Called by CollectableSystem so that the collectable gets initialized
    // whether this gameObject is enabled or disabled.
    public void Initialize()
    {
        if (initialized)
        {
            return;
        }

        initialized = true;
        startingPosIdentifier = transform.position;

        anim = GetComponent<Animator>();
        collected = CollectibleSystem.collected.Contains(startingPosIdentifier);

        //initialize the variables
        collectibleCollider = GetComponent<Collider>();

        if (tempForceNotCollected)
        {
            collected = false;
        }
        else if (collected)
        {
            CollectibleSystem.instance.AutoCollectOnLoad(this);
            onAutoCollectAtLoad.Invoke();
        }

        collectibleCollider.enabled = !collected;
        overheadVisual.SetActive(!collected);
        collectibleVisual.SetActive(!collected);

        overheadVisual.transform.SetParent(null);
        overheadVisual.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
    }

    //Update the Collectible Reader
    public void UpdateCollectibleReader()
    {
        collectibleCollider.enabled = !collected;
        collectibleVisual.SetActive(!collected);
        overheadVisual.SetActive(!collected);
    }

    //Returns the collectible
    public Collectible GetCollectible()
    {
        return collectible;
    }

    //Sets collected to true
    public void CollectObject()
    {
        if (!collected)
        {
            collected = true;
            onCollect.Invoke();

            if (GetComponentInParent<CollectWanderLeafScript>() != null)
            {
                GetComponentInParent<CollectWanderLeafScript>().startCollect();
                Invoke("UpdateCollectibleReader", 5f);
            }
            else if (playAnimation && anim != null)
            {
                Invoke("UpdateCollectibleReader", animLength);
            }
            if (playAnimation && anim != null)
            {
                anim.SetTrigger("Collected");
            }
            else
            {
                UpdateCollectibleReader();
            }
        }
    }


    //returns whether or not this collectible has been collected
    public bool IsCollected()
    {
        return collected;
    }
}
