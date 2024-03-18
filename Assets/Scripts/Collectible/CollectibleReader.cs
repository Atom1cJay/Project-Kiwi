using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(Collider))]
public class CollectibleReader : MonoBehaviour
{
    public Collectible collectible;

    public bool tempForceNotCollected;

    Collider collectibleCollider;

    public bool collected;

    public string name;

    [SerializeField] GameObject collectibleVisual;

    [SerializeField] bool playAnimation;
    [SerializeField] GameObject overheadVisual;

    Animator anim; // Optional! (Used if playAnimation is true)
    [SerializeField] float animLength; // Optional! (Used if playAnimation is true)

    public UnityEvent onCollect;

    // Start is called before the first frame supdate
    void Start()
    {
        anim = GetComponent<Animator>();
        collected = CollectibleSystem.collected.Contains(gameObject.transform.position);

        //initialize the variables
        collectibleCollider = GetComponent<Collider>();

        if (tempForceNotCollected)
            collected = false;

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
