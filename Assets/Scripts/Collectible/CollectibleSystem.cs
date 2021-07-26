using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSystem : MonoBehaviour
{

    [SerializeField] static int coinCount, blueCoinCount, starCount;


    // Update is called once per frame
    void Update()
    {
        //Debug.Log("coins:" + coinCount);
        

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


    //Return the number of coins you have for the given Collectible Type
    public int GetCount(Collectible.CollectibleType type)
    {

        if (type == Collectible.CollectibleType.COIN)
        {
            return coinCount;
        }
        else if (type == Collectible.CollectibleType.BLUECOIN)
        {
            return blueCoinCount;
        }
        else if (type == Collectible.CollectibleType.STAR)
        {
            return starCount;
        }
        else
            return 0;
    }
    public void CollectItem(CollectibleReader cr, int i = 1)
    {
        Debug.Log("collected item");

        Collectible.CollectibleType type = cr.GetCollectible().GetType();

        if (type == Collectible.CollectibleType.COIN)
        {
            coinCount += i;
        }
        else if (type == Collectible.CollectibleType.BLUECOIN)
        {
            blueCoinCount += i;
        }
        else if (type == Collectible.CollectibleType.STAR)
        {
            starCount += i;
        }

        cr.CollectObject();
    }


}
