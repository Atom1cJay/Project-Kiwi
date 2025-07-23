using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Damages the player if they are standing on a shadow.
public class ShadowDamager : MonoBehaviour
{
    [SerializeField] DamageType damageType;
    [SerializeField] PlayerHealth ph;
    [SerializeField] MovementInfo mi;
    [SerializeField] Transform player;
    [SerializeField] LayerMask raycastLayerMask;
    [SerializeField] Transform directionalLight;

    void Update()
    {
        Debug.DrawRay(player.position, -directionalLight.transform.forward);
        if (mi.TouchingGround() && Physics.Raycast(player.position, -directionalLight.transform.forward, 100, raycastLayerMask))
        {
            ph.HandleDamage(damageType, Vector3.zero);
        }
    }
}
