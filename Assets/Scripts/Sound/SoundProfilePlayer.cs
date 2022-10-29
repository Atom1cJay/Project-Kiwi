using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundProfilePlayer : MonoBehaviour
{
    [SerializeField] FinishableOccurrence occurrence;
    [SerializeField] SoundProfile soundProfile;
    bool occurrenceIsActive = false;

    void Start()
    {
        occurrence.OnOccurrenceStart += () =>
        {
            soundProfile.Initiate();
            occurrenceIsActive = true;
            StartCoroutine("AdvanceTimeForSoundProfile");
        };
        occurrence.OnOccurrenceFinish += () =>
        {
            soundProfile.Finish();
            occurrenceIsActive = false;
        };
    }

    IEnumerator AdvanceTimeForSoundProfile()
    {
        while (occurrenceIsActive)
        {
            soundProfile.AdvanceTime();
            yield return null;
        }
    }
}
