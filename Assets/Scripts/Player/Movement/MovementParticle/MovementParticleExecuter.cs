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
        //me.OnMoveChanged.AddListener(() => HandleMoveChange(me.GetCurrentMove()));
    }

    private void Update()
    {
        MovementParticleInfo.MovementParticles[] particles = me.GetCurrentMove().GetParticlesToSpawn();
        if (particles != null) {
            foreach (MovementParticleInfo.MovementParticles p in particles) {
                SpawnParticle(p);
            }
        }
    }

    /*
    void HandleMoveChange(IMoveImmutable move)
    {
        MovementParticleInfo.MovementParticles[] particles = move.GetParticlesToSpawn();
        if (particles != null)
        {
            foreach (MovementParticleInfo.MovementParticles p in particles) {
                SpawnParticle(p);
            }
        }
    }
    */

    void SpawnParticle(MovementParticleInfo.MovementParticles info)
    {
        GameObject spawned;

        // Should particles be a child of the player or be on their own?
        if (info.staysWithPlayer)
        {
            spawned = Instantiate(info.particles, transform);
        }
        else
        {
            // Offset in the prefabs section should be applied here
            Vector3 offset = info.particles.transform.position;
            spawned = Instantiate(info.particles, transform.position + offset, transform.rotation);
        }

        // Handle particle despawning
        if (info.maxTimeBeforeDestroy > 0)
        {
            // For timed destruction or (if applicable) post-halt destruction
            StartCoroutine(HandleParticleLifetime(info.maxTimeBeforeDestroy, info.haltedByNewMove, spawned, info.timeAfterHaltToDestroy));
        }
        else if (info.haltedByNewMove && info.timeAfterHaltToDestroy > 0)
        {
            // For post-halt destruction
            StartCoroutine(HandleParticleLifetime(float.MaxValue, info.haltedByNewMove, spawned, info.timeAfterHaltToDestroy));
        }
        else
        {
            // No prompt for destruction can exist
            Debug.LogError("Particle system " + info + " could never be destroyed. Check its attributes.");
        }
    }

    // Determines what to do during the particle lifetime after it has spawned.
    // This includes destroying it when necessary, and halting it when necessary.
    IEnumerator HandleParticleLifetime(float lifetime, bool newMoveHalts, GameObject particles, float timeAfterHaltToDestroy)
    {
        bool moveHalted = false;
        bool newMove = false;
        me.OnMoveChanged.AddListener(() => { newMove = true; });
        float timeLeftToLive = lifetime;
        while (timeLeftToLive > 0)
        {
            if (newMove && newMoveHalts && !moveHalted)
            {
                moveHalted = true;
                HaltAllParticlesFoundInGameObj(particles);
                if (timeAfterHaltToDestroy >= 0)
                {
                    timeLeftToLive = timeAfterHaltToDestroy;
                }
            }
            timeLeftToLive -= Time.deltaTime;
            yield return null;
        }
        Destroy(particles);
    }

    // For all particle systems found in the given gameObject OR its children,
    // stop the particle system
    void HaltAllParticlesFoundInGameObj(GameObject go)
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
