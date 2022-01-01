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
            Vector3 hitNormal = other.GetContact(0).normal;
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
