using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collectible", menuName ="Collectible")]
public class Collectible : ScriptableObject
{
    public enum CollectibleType { COIN, BLUECOIN, STAR };

    [SerializeField] CollectibleType type;

    [SerializeField] Mesh mesh;

    [SerializeField] string name;

    [SerializeField] int num = 0;

    public Collectible(Collectible c)
    {
        this.type = c.GetType();
        this.mesh = c.mesh;
        this.name = c.GetName() + " copy";
        this.num = c.GetNum();
    }

    //returns the mesh
    public Mesh GetMesh()
    {
        return mesh;
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
