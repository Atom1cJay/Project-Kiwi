using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IntervalSoundProfile : ContinuousSoundProfile
{
    [SerializeField] float averageIntervalSize;
    [SerializeField] float intervalSizeRange;
    float currentIntervalProgress = 0;
    float currentIntervalSize;

    void PlaySoundAndResetInterval()
    {
        PlayNextSound();
        currentIntervalProgress = 0;
        currentIntervalSize = averageIntervalSize + Random.Range(-intervalSizeRange, intervalSizeRange);
    }

    public override void Initiate()
    {
        PlaySoundAndResetInterval();
    }

    protected void ContributeToInterval(float amount)
    {
        currentIntervalProgress += amount;
        if (currentIntervalProgress > currentIntervalSize)
        {
            PlaySoundAndResetInterval();
        }
    }
}
