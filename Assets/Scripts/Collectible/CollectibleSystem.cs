using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleSystem : MonoBehaviour
{
    [HideInInspector]
    public static CollectibleSystem instance;

    [HideInInspector]
    public static List<Vector3> collected = new List<Vector3>();

    [SerializeField] TextMeshProUGUI seedDisplay, blueSeedDisplay, wanderLeafDisplay;

    [SerializeField] static int seedCount, blueSeedCount, wanderLeafCount;

    [SerializeField] Sound collectSeed, collectBlueSeed, collectWanderLeaf;

    [SerializeField] WanderLeafCollectionProgressUI collectionProgress;


    // Update is called once per frame
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of CollectibleSystem exist. Only have one.");
        }
        instance = this;
    }

    private void Start()
    {
        Debug.Log("Collected: " + collected.Count);
    }

    //OnTriggerEnter
    void OnTriggerEnter (Collider col)
    {
        CollectibleReader cr = col.gameObject.GetComponent<CollectibleReader>();
        if (cr != null && !cr.collected)
        {
            CollectItem(cr);
            cr.CollectObject();
        }
    }

    public void CollectItem(CollectibleReader cr, int i = 1)
    {
        Debug.Log("collected item");
        CollectibleSystem.collected.Add(cr.gameObject.transform.position);
        //Debug.Log("updated c : " + CollectibleSystem.collected.Count);

        Collectible.CollectibleType type = cr.GetCollectible().GetType();

        if (type == Collectible.CollectibleType.SEED)
        {
            seedCount += i;
            AudioMasterController.instance.PlaySound(collectSeed);
        }
        else if (type == Collectible.CollectibleType.BLUESEED)
        {
            blueSeedCount += i;
            AudioMasterController.instance.PlaySound(collectBlueSeed);
            collectionProgress.collectWanderLeaf(1);
        }
        else if (type == Collectible.CollectibleType.WANDERLEAF)
        {
            wanderLeafCount += i;
            AudioMasterController.instance.PlaySound(collectWanderLeaf);
            collectionProgress.collectWanderLeaf(5);
        }


        seedDisplay.SetText(seedCount.ToString());
        blueSeedDisplay.SetText(blueSeedCount.ToString());
        wanderLeafDisplay.SetText(wanderLeafCount.ToString());

        cr.CollectObject();
    }


}
