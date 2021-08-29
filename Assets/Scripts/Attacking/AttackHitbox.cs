using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] AttackType attackType;

    public AttackType GetAttackType()
    {
        return attackType;
    }
}
