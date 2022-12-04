using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gives a gameObject the ability to damage the player.
/// NOTE: This script will change the collision layer of the object to
/// "Damaging".
/// </summary>
public class Damager : MonoBehaviour
{
    [SerializeField] DamageType damageType;
    [SerializeField] bool ignoreInvulnerability;
    [SerializeField] bool destroyOnContact = false;
    bool isActivated = true;

    private void Start()
    {
        gameObject.layer = ignoreInvulnerability ? LayerMask.NameToLayer("DamagingIgnoreInv") : LayerMask.NameToLayer("Damaging");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 hitNormal = transform.position - other.ClosestPointOnBounds(transform.position);
            ConsiderDmg(hitNormal);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 hitNormal = other.GetContact(0).normal;
            ConsiderDmg(hitNormal);
        }

        if (destroyOnContact)
        {
            Destroy(gameObject);
        }
    }

    void ConsiderDmg(Vector3 hitNormal)
    {
        if (isActivated)
        {
            hitNormal.x = -hitNormal.x;
            hitNormal.z = -hitNormal.z;
            PlayerHealth.instance.HandleDamage(damageType, hitNormal);
        }
    }

    public void SetActivated(bool activated)
    {
        isActivated = activated;
    }

    /// <summary>
    /// Disables the damager, and reenables it soon after.
    /// IMPORTANT: You shouldn't call SetActivated()
    /// until the damager has been enabled again.
    /// </summary>
    public void DisableTemp()
    {
        StartCoroutine("TempDisableCo");
    }

    IEnumerator TempDisableCo()
    {
        isActivated = false;
        yield return new WaitForSeconds(0.05f);
        isActivated = true;
    }
}
