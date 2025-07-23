using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Script for a gold ring item
// Allows for challenges where the player has a limited amount of
// time to travel from one ring to the next, to the next, and so on
// until they reach the last one and win.
public class GoldRing : MonoBehaviour
{
    [SerializeField] GoldRing firstRing; // Defines the "ring group"
    [SerializeField] GoldRing nextRing; // If this is the last, leave null
    [SerializeField] GameObject collectionParticles;
    [SerializeField] float timeLimitForNextRing;
    [SerializeField] GameObject reward;
    [SerializeField] Sound clockSound;
    [SerializeField] Sound standardRingSound;
    [SerializeField] Sound winRingSound;
    [SerializeField] UnityEvent collectedEvent;

    void Start()
    {
        if (firstRing == this)
        {
            reward.SetActive(false);
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
            if (next.firstRing != this)
            {
                Debug.LogError("FirstRing is inconsistent in sequence.");
                return;
            }
            if (next.reward != this.reward)
            {
                Debug.LogError("Reward isn't consistent in sequence");
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
            AudioMasterController.instance.PlaySound(standardRingSound);
            AudioMasterController.instance.PlaySound(clockSound, timeLimitForNextRing);
            nextRing.gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("PersonalArrow").GetComponent<PersonalArrow>().ShowArrow(nextRing.transform, timeLimitForNextRing);
            MonobehaviourUtils.Instance.StartCoroutine(ConsiderRespawn(timeLimitForNextRing, firstRing, nextRing));
        }
        else // This is the last ring, you win!
        {
            AudioMasterController.instance.PlaySound(winRingSound);
            AudioMasterController.instance.StopSound(clockSound.GetName());
            reward.SetActive(true);
        }

        collectedEvent.Invoke();
        gameObject.SetActive(false);
    }

    static IEnumerator ConsiderRespawn(float secondsToWait, GoldRing firstRing, GoldRing ringToCheck)
    {
        yield return new WaitForSeconds(secondsToWait);
        if (ringToCheck.gameObject.activeSelf) // Has not been collected and destroyed
        {
            ringToCheck.gameObject.SetActive(false);
            firstRing.gameObject.SetActive(true);
        }
    }
}
