using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AIProfile", menuName = "AIProfile")]
public class AIProfile : ScriptableObject
{

    public float AggroDistance;
    public int InitialHealth;
    public float ITime;
    public int Damage;
    public float KnockbackGiven;


}

