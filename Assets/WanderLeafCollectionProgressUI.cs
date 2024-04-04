using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WanderLeafCollectionProgressUI : MonoBehaviour
{
    [SerializeField] Image progressImage;
    [SerializeField] int goal;
    [SerializeField] float timeToLerp;

    int currentCount;

    public void collectWanderLeaf(int increment)
    {
        StartCoroutine(lerpToNewGoal(currentCount + increment));
        currentCount += increment;
    }


    IEnumerator lerpToNewGoal(int newAmount)
    {
        int prevCount = currentCount;

        float startTime = Time.time;

        while (Time.time < startTime + timeToLerp)
        {
            yield return null;
            float pct = (Time.time - startTime) / timeToLerp;
            progressImage.fillAmount = Mathf.Lerp((float)prevCount / (float)goal, (float)newAmount / (float)goal, pct);
        }

    }
}
