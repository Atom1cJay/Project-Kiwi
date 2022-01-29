using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collectible", menuName ="Collectible")]
public class Collectible : ScriptableObject
{
    public enum CollectibleType { SEED, BLUESEED, WANDERLEAF };

    [SerializeField] CollectibleType type;

    [SerializeField] string name;

    [SerializeField] int num = 0;

    public Collectible(Collectible c)
    {
        this.type = c.GetType();
        this.name = c.GetName() + " copy";
        this.num = c.GetNum();
    }

    //returns the type of the collectible
    public CollectibleType GetType()
    {
        return type;
    }

    //gets the name of the collectible
    public string GetName()
    {
        return name;
    }

    //gets the num of the collectible
    public int GetNum()
    {
        return num;
    }
}
