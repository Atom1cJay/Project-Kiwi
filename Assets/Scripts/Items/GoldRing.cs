using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for a gold ring item
// Allows for challenges where the player has a limited amount of
// time to travel from one ring to the next, to the next, and so on
// until they reach the last one and win.
public class GoldRing : MonoBehaviour
{
    [SerializeField] bool isFirstRing;
    [SerializeField] GoldRing nextRing; // If this is the last, leave null
    [SerializeField] GameObject collectionParticles;

    void Start()
    {
        if (isFirstRing)
        {
            EnsureSequenceValidity();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Confirm the ring sequence starting from this one is valid (only one "first" ring, not circular)
    void EnsureSequenceValidity()
    {
        List<GoldRing> ringsInSequence = new List<GoldRing>();
        ringsInSequence.Add(this);
        GoldRing next = nextRing;
        while (next != null)
        {
            if (next.isFirstRing)
            {
                Debug.LogError("Multiple 'first' rings in ring sequence.");
                return;
            }
            if (ringsInSequence.Contains(next))
            {
                Debug.LogError("Ring sequence is circular.");
                return;
            }
            ringsInSequence.Add(next);
            next = next.nextRing;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (collectionParticles != null)
        {
            GameObject particles = Instantiate(collectionParticles);
            particles.transform.position = transform.position;
        }
        if (nextRing != null)
        {
            nextRing.gameObject.SetActive(true);
        }
        Destroy(gameObject);
    }
}
