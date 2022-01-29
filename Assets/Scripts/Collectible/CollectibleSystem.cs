using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleSystem : MonoBehaviour
{
    [HideInInspector]
    public static CollectibleSystem instance;

    [SerializeField] TextMeshProUGUI seedDisplay, blueSeedDisplay, wanderLeafDisplay;

    [SerializeField] static int seedCount, blueSeedCount, wanderLeafCount;


    // Update is called once per frame
    void Start()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of CollectibleSystem exist. Only have one.");
        }
        instance = this;
    }

    //OnTriggerEnter
    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.GetComponent<CollectibleReader>() != null)
        {
            CollectibleReader cr = col.gameObject.GetComponent<CollectibleReader>();
            CollectItem(cr);
            cr.CollectObject();
        }
    }

    public void CollectItem(CollectibleReader cr, int i = 1)
    {
        Debug.Log("collected item");

        Collectible.CollectibleType type = cr.GetCollectible().GetType();

        if (type == Collectible.CollectibleType.SEED)
        {
            seedCount += i;
        }
        else if (type == Collectible.CollectibleType.BLUESEED)
        {
            blueSeedCount += i;
        }
        else if (type == Collectible.CollectibleType.WANDERLEAF)
        {
            wanderLeafCount += i;
        }


        seedDisplay.SetText(seedCount.ToString());
        blueSeedDisplay.SetText(blueSeedCount.ToString());
        wanderLeafDisplay.SetText(wanderLeafCount.ToString());

        cr.CollectObject();
    }


}
