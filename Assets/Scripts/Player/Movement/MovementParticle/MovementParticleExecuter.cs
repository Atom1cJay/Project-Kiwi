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
        MovementParticleInfo.MovementParticles particles = move.GetParticlesToSpawn();
        GameObject spawned;
        if (particles != null)
        {
            // Handle particle spawning
            // Should particles be a child of the player or be on their own?
            if (particles.staysWithPlayer)
            {
                spawned = Instantiate(particles.particles, transform);
            }
            else
            {
                // Offset in the prefabs section should be applied here
                Vector3 offset = particles.particles.transform.position;
                spawned = Instantiate(particles.particles, transform.position + offset, transform.rotation);
            }

            // Handle particle despawning
            if (particles.maxTimeBeforeDestroy > 0)
            {
                StartCoroutine(KillParticles(particles.maxTimeBeforeDestroy, particles.destroyedByNewMove, particles.haltedByNewMove, spawned));
            }
            else
            {
                StartCoroutine(KillParticles(float.MaxValue, particles.destroyedByNewMove, particles.haltedByNewMove, spawned));
            }
        }
    }

    // Destroy the given particles when the lifetime has been completed, or
    // another move takes place; whichever comes first
    IEnumerator KillParticles(float lifetime, bool newMoveDestroys, bool newMoveHalts, GameObject particles)
    {
        bool newMove = false;
        float timeLeftToLive = lifetime;
        me.OnMoveChanged.AddListener(() => {
            if (newMoveDestroys) { timeLeftToLive = 0; }
            newMove = true; } );
        while (timeLeftToLive > 0)
        {
            if (newMove && newMoveHalts)
            {
                HaltAllParticlesFoundInGameObj(particles);
            }
            timeLeftToLive -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
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
