using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collectible", menuName ="Collectible")]
public class Collectible : ScriptableObject
{
    public enum CollectibleType { SEED, BLUESEED, WANDERLEAF };

    [SerializeField] CollectibleType type;

    [SerializeField] int num = 0;

    public Collectible(Collectible c)
    {
        this.type = c.GetType();
        this.num = c.GetNum();
    }

    //returns the type of the collectible
    public CollectibleType GetType()
    {
        return type;
    }

    //gets the num of the collectible
    public int GetNum()
    {
        return num;
    }
}
