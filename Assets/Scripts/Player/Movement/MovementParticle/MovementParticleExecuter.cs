using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveExecuter))]
public class MovementParticleExecuter : MonoBehaviour
{
    MoveExecuter me;

    struct Spawn
    {
        public GameObject prefab { get; private set; }
        public GameObject instantiated { get; private set; }

        public Spawn(GameObject prefab, GameObject instantiated)
        {
            this.prefab = prefab;
            this.instantiated = instantiated;
        }
    }

    List<Spawn> activeSpawns;

    void Awake()
    {
        me = GetComponent<MoveExecuter>();
        activeSpawns = new List<Spawn>();
    }

    /// <summary>
    /// Every frame, check which particles are supposed to spawn and despawn,
    /// and update accordingly.
    /// </summary>
    private void Update()
    {
        MovementParticleInfo.MovementParticles[] particles = MoveExecuter.GetCurrentMove().GetParticlesToSpawn();
        if (particles != null)
        {
            foreach (MovementParticleInfo.MovementParticles p in particles)
            {
                SpawnParticle(p);
            }
        }
        MovementParticleInfo.MovementParticles[] toStop = MoveExecuter.GetCurrentMove().GetParticlesToStop();
        if (toStop != null)
        {
            foreach (MovementParticleInfo.MovementParticles p in toStop)
            {
                StopParticle(p);
            }
        }
    }

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

        activeSpawns.Add(new Spawn(info.particles, spawned));
    }

    // If the particles represented by the given info struct are currently active, halt it.
    void StopParticle(MovementParticleInfo.MovementParticles info)
    {
        Spawn toDelete = new Spawn(null, null); // Placeholder

        foreach (Spawn s in activeSpawns)
        {
            if (s.prefab == info.particles)
            {
                if (s.instantiated != null)
                {
                    HaltAllParticlesFoundInGameObj(s.instantiated);
                    toDelete = s;
                    break;
                }
            }
        }

        if (toDelete.instantiated != null) // If not placeholder (actual value to delete)
        {
            activeSpawns.Remove(toDelete);
        }
    }

    // Determines what to do during the particle lifetime after it has spawned.
    // This includes destroying it when necessary, and halting it when necessary.
    IEnumerator HandleParticleLifetime(float lifetime, bool newMoveHalts, GameObject particles, float timeAfterHaltToDestroy)
    {
        bool moveHalted = false;
        bool newMove = false;
        MoveExecuter.instance.OnMoveChanged += (_, _) => newMove = true;
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
