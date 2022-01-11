using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionChain : MonoBehaviour
{
    [Serializable]
    public struct SoundDelay
    {
        public float delay;
        public string soundName;
    }

    private StateController StateChecker;
    bool stopChain = false;

    [SerializeField] List<SoundDelay> chainedTimes;

    private void Awake()
    {
        StateChecker = GetComponent<StateController>();
    }

    public void Update()
    {

        if (StateChecker.StateCheck())
        {
            StartCoroutine(Play());
                
        }


    }

    IEnumerator Play()
    {
        stopChain = false;
        foreach (SoundDelay sd in chainedTimes)
        {
            yield return new WaitForSeconds(sd.delay);
            if (!stopChain)
            {
                AudioMasterController.instance.PlaySound(sd.soundName);
            }
        }

        stopChain = false;

    }

    public void StopChain()
    {
        stopChain = true;
    }
}
