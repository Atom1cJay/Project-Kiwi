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

    [SerializeField] int leafPower, blueSeedPower;

    [SerializeField] Sound collectSeed, collectBlueSeed, collectWanderLeaf;

    [SerializeField] WanderLeafCollectionProgressUI collectionProgress;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of CollectibleSystem exist. Only have one.");
            return;
        }
        instance = this;
        foreach (CollectibleReader cr in FindObjectsOfType<CollectibleReader>(true))
        {
            cr.Initialize(); // Called here so that they initialize whether enabled or disabled
        }
    }

    /*
    private void Start()
    {
        Debug.Log("Collected: " + collected.Count);
    }
    */

    void OnTriggerEnter(Collider col)
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
        //Debug.Log("collected item");
        CollectibleSystem.collected.Add(cr.startingPosIdentifier);
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
            collectionProgress.collectWanderLeaf(blueSeedPower);
        }
        else if (type == Collectible.CollectibleType.WANDERLEAF)
        {
            wanderLeafCount += i;
            AudioMasterController.instance.PlaySound(collectWanderLeaf);
            collectionProgress.collectWanderLeaf(leafPower);
        }

        seedDisplay.SetText(seedCount.ToString());
        blueSeedDisplay.SetText(blueSeedCount.ToString());
        wanderLeafDisplay.SetText(wanderLeafCount.ToString());

        cr.CollectObject();
    }

    public void AutoCollectOnLoad(CollectibleReader cr)
    {
        Collectible.CollectibleType type = cr.GetCollectible().GetType();

        if (type == Collectible.CollectibleType.BLUESEED)
        {
            collectionProgress.collectWanderLeaf(blueSeedPower, true);
        }
        else if (type == Collectible.CollectibleType.WANDERLEAF)
        {
            collectionProgress.collectWanderLeaf(leafPower, true);
        }
    }
}
