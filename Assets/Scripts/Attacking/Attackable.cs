using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles attacks dealt towards something. If part of an entity can be
/// attacked and part of it can't, this script should be placed specifically
/// on the part that can.
/// </summary>
public class Attackable : MonoBehaviour
{
    [SerializeField] UnityEvent jumpAttackEvent;
    [SerializeField] UnityEvent diveAttackEvent;
    [SerializeField] UnityEvent horizBoostAttackEvent;
    [SerializeField] UnityEvent vertBoostAttackEvent;
    [SerializeField] UnityEvent groundPoundAttackEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            AttackHitbox hitbox = other.gameObject.GetComponent<AttackHitbox>();
            if (hitbox == null)
            {
                Debug.LogError("Hitbox not detected, even though the layer of " +
                    "the object that hit this Attackable is the hitbox layer!");
                return;
            }

            switch (hitbox.GetAttackType())
            {
                case AttackType.Jump:
                    jumpAttackEvent.Invoke();
                    print("jumped on");
                    break;
                case AttackType.Dive:
                    diveAttackEvent.Invoke();
                    print("dived into");
                    break;
                case AttackType.HorizBoost:
                    horizBoostAttackEvent.Invoke();
                    print("horiz boosted into");
                    break;
                case AttackType.VertBoost:
                    vertBoostAttackEvent.Invoke();
                    print("vert boosted into");
                    break;
                case AttackType.GroundPound:
                    groundPoundAttackEvent.Invoke();
                    print("ground pounded into");
                    break;
                default:
                    Debug.LogError("The hitbox that hit this attackable has an unidentified attack type.");
                    break;
            }
        }
    }
}
