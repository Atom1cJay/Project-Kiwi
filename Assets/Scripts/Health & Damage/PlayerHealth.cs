using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    [SerializeField] int hp;
    [SerializeField] float iFrameDuration;
    bool isInvulnerable;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of PlayerHealth exist. Only have one.");
        }
        instance = this;
    }

    /// <summary>
    /// Takes damage of the specified type to the player, unless invulnerable.
    /// </summary>
    /// <param name="damageType"></param>
    public void HandleDamage(DamageType damageType)
    {
        if (isInvulnerable || hp <= 0)
        {
            return;
        }

        switch (damageType)
        {
            case DamageType.BasicHit:
                print("BASIC HIT TAKEN");
                hp--;
                break;
            case DamageType.Instakill:
                print("INSTAKILL HIT TAKEN");
                hp = 0;
                break;
            case DamageType.FallKill:
                print("FALL HIT TAKEN");
                hp = 0;
                break;
            default:
                Debug.LogError("Cannot take damage; unrecognized dmg type.");
                break;
        }

        if (hp <= 0)
        {
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
        isInvulnerable = true;
        yield return new WaitForSeconds(iFrameDuration);
        isInvulnerable = false;
    }
}
