using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPathFollower : PathFollower
{
    [SerializeField] float thunderSpeedMultiplier;
    [SerializeField] float thunderSpeedDuration;
    bool inThunderSpeed;

    public void ActivateThunderSpeed()
    {
        if (inThunderSpeed) return;
        StartCoroutine(ThunderSpeed());
    }

    IEnumerator ThunderSpeed()
    {
        // Initial Burst
        float initSpeed = speed;
        speed *= thunderSpeedMultiplier;
        float boostedSpeed = speed;
        inThunderSpeed = true;
        // Slowdown
        for (float t = 0; t < thunderSpeedDuration; t += Time.deltaTime)
        {
            speed = Mathf.Lerp(boostedSpeed, initSpeed, t / thunderSpeedDuration);
            yield return null;
        }
        speed = initSpeed;
        inThunderSpeed = false;
    }
}
