using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gives a gameObject the ability to damage the player.
/// NOTE: Requires that this object have a trigger collider.
/// </summary>
public class Damager : MonoBehaviour
{
    [SerializeField] DamageType damageType;
    bool isActivated = true;

    private void OnCollisionStay(Collision other)
    {
        if (isActivated)
        {
            PlayerHealth.instance.HandleDamage(damageType);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isActivated)
        {
            PlayerHealth.instance.HandleDamage(damageType);
        }
    }

    public void SetActivated(bool activated)
    {
        isActivated = activated;
    }
}
