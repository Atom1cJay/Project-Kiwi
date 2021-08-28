using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    BasicHit, // Standard knockback
    Instakill, // Standard knockback, results in death
    FallKill // No knockback, results in death
}
