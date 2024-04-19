using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Deals with hits to the player's health and responses to it.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public int hp { get; private set; }
    [SerializeField] int startingHP;
    [SerializeField] float iFrameDuration;
    public UnityEvent onHealthChanged { get; private set; }
    public Vector3Event onBasicHit { get; private set; }
    public UnityEvent onYeet { get; private set; }
    public UnityEvent onDeath { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of PlayerHealth exist. Only have one.");
        }
        instance = this;
        hp = startingHP;
        onHealthChanged = new UnityEvent();
        onBasicHit = new Vector3Event();
        onDeath = new UnityEvent();
        onYeet = new UnityEvent();
        // Do NOT ignore collision between these two layers
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player Collision"), LayerMask.NameToLayer("Damaging"), false);
    }

    /// <summary>
    /// Takes damage of the specified type to the player, if the conditions are
    /// right for damage to take place.
    /// ASSUMED: This method will only get called if the player is either invulnerable,
    /// or the damaging object ignores invulnerability.
    /// </summary>
    /// <param name="damageType">The type of damage relevant to the hit being (potentially) taken</param>
    public void HandleDamage(DamageType damageType, Vector3 normalOfContact)
    {
        if (hp <= 0)
        {
            return;
        }

        switch (damageType)
        {
            case DamageType.BasicHit:
                onBasicHit.Invoke(normalOfContact);
                print("BASIC HIT TAKEN");
                hp--;
                break;
            case DamageType.Instakill:
                onBasicHit.Invoke(normalOfContact);
                print("INSTAKILL HIT TAKEN");
                hp = 0;
                break;
            case DamageType.FallKill:
                print("FALL HIT TAKEN");
                hp = 0;
                break;
            case DamageType.Knockback:
                onBasicHit.Invoke(normalOfContact);
                print("KNOCKBACK HIT TAKEN");
                break;
            case DamageType.Yeet:
                onYeet.Invoke();
                print("YEET HIT TAKEN");
                break;
            default:
                Debug.LogError("Cannot take damage; unrecognized dmg type.");
                break;
        }

        onHealthChanged.Invoke();

        if (hp <= 0)
        {
            onDeath.Invoke();
            print("YOU DIED."); // TODO actual death sequence
        }
        else
        {
            StartCoroutine(IFrames());
        }
    }

    /// <summary>
    /// Handles invulnerability frames.
    /// </summary>
    IEnumerator IFrames()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player Collision"), LayerMask.NameToLayer("Damaging"), true);
        yield return new WaitForSeconds(iFrameDuration);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player Collision"), LayerMask.NameToLayer("Damaging"), false);
    }
}
