using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WanderLeafCollectionProgressUI : MonoBehaviour
{
    [SerializeField] Image progressImage;
    [SerializeField] Image bgImage;
    [SerializeField] int goal;
    [SerializeField] float timeToLerp;
    [SerializeField] Color collectionColorA;
    [SerializeField] Color collectionColorB;
    [SerializeField] float collectionFlashSpeed;
    [SerializeField] UnityEvent onFilled;
    [SerializeField] UnityEvent onFilledAtLoad;
    bool calledOnFilledEvent;
    Color initBGColor;

    int currentCount;

    private void Start()
    {
        initBGColor = bgImage.color;
    }

    public void collectWanderLeaf(int increment, bool onLoad=false)
    {
        if (onLoad)
        {
            ConsiderFillEvent(currentCount + increment, true);
            progressImage.fillAmount = (float)(currentCount + increment) / goal;
        }
        else
        {
            StartCoroutine(lerpToNewGoal(currentCount, currentCount + increment));
        }
        currentCount += increment;
    }

    IEnumerator lerpToNewGoal(int prevCount, int newAmount)
    {
        ConsiderFillEvent(newAmount);
        float startTime = Time.time;

        while (Time.time < startTime + timeToLerp)
        {
            yield return null;
            float elapsed = Time.time - startTime;
            bgImage.color = Color.Lerp(collectionColorA, collectionColorB, Mathf.Sin(elapsed * collectionFlashSpeed));
            float pct = (Time.time - startTime) / timeToLerp;
            progressImage.fillAmount = Mathf.Lerp((float)prevCount / (float)goal, (float)newAmount / (float)goal, pct);
        }
        bgImage.color = initBGColor;
    }

    void ConsiderFillEvent(int newAmount, bool onLoad=false)
    {
        if (newAmount >= goal && !calledOnFilledEvent)
        {
            UnityEvent fillEvent = onLoad ? onFilledAtLoad : onFilled;
            fillEvent.Invoke();
            calledOnFilledEvent = true;
        }
    }
}
