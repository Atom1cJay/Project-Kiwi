using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveExecuter))]
public class MovementParticleExecuter : MonoBehaviour
{
    MoveExecuter me;

    void Awake()
    {
        me = GetComponent<MoveExecuter>();
        me.OnMoveChanged.AddListener(() => HandleParticleChange(me.GetCurrentMove()));
    }

    void HandleParticleChange(IMoveImmutable move)
    {
        MovementParticleInfo.MovementParticles[] particles = move.GetParticlesToSpawn();
        GameObject spawned;
        if (particles != null)
        {
            foreach (MovementParticleInfo.MovementParticles p in particles) {
                // Handle particle spawning
                // Should particles be a child of the player or be on their own?
                if (p.staysWithPlayer)
                {
                    spawned = Instantiate(p.particles, transform);
                }
                else
                {
                    // Offset in the prefabs section should be applied here
                    Vector3 offset = p.particles.transform.position;
                    spawned = Instantiate(p.particles, transform.position + offset, transform.rotation);
                }

                // Handle particle despawning
                if (p.maxTimeBeforeDestroy > 0)
                {
                    // For timed destruction or (if applicable) post-halt destruction
                    StartCoroutine(HandleParticleLifetime(p.maxTimeBeforeDestroy, p.haltedByNewMove, spawned, p.timeAfterHaltToDestroy));
                }
                else if (p.haltedByNewMove && p.timeAfterHaltToDestroy > 0)
                {
                    // For post-halt destruction
                    StartCoroutine(HandleParticleLifetime(float.MaxValue, p.haltedByNewMove, spawned, p.timeAfterHaltToDestroy));
                }
                else
                {
                    // No prompt for destruction can exist
                    Debug.LogError("Particle system " + p + " could never be destroyed. Check its attributes.");
                }
            }
        }
    }

    // Determines what to do during the particle lifetime after it has spawned.
    // This includes destroying it when necessary, and halting it when necessary.
    IEnumerator HandleParticleLifetime(float lifetime, bool newMoveHalts, GameObject particles, float timeAfterHaltToDestroy)
    {
        bool newMove = false;
        me.OnMoveChanged.AddListener(() => { newMove = true; });
        float timeLeftToLive = lifetime;
        while (timeLeftToLive > 0)
        {
            if (newMove && newMoveHalts)
            {
                HaltAllParticlesFoundInGameObj(particles, timeAfterHaltToDestroy);
                lifetime = timeAfterHaltToDestroy;
            }
            timeLeftToLive -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(particles);
    }

    // For all particle systems found in the given gameObject OR its children,
    // stop the particle system
    void HaltAllParticlesFoundInGameObj(GameObject go, float timeAfterHaltToDestroy)
    {
        ParticleSystem[] allPS = go.GetComponentsInChildren<ParticleSystem>();
        if (allPS.Length != 0)
        {
            foreach(ParticleSystem ps in allPS)
            {
                ps.Stop();
            }
        }
    }
}
