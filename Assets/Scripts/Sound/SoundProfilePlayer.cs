using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundProfilePlayer : MonoBehaviour
{
    [SerializeField] SoundProfile soundProfile;
    bool occurrenceIsActive = false;

    public void StartSoundProfileExecution()
    {
        soundProfile.Initiate();
        occurrenceIsActive = true;
        StartCoroutine("AdvanceTimeForSoundProfile");
    }

    public void FinishSoundProfileExecution()
    {
        soundProfile.Finish();
        occurrenceIsActive = false;
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
