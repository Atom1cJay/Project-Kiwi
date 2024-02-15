using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationAnimation : MonoBehaviour
{
    [SerializeField] GameObject gotoPosition;
    [SerializeField] float timeToGetToPos, timeToReturnToPos;

    Vector3 initialPos;
    Vector3 goalPos;

    public void startAnimation()
    {
        if (initialPos == null)
            gameObject.transform.position = initialPos;
        else
            initialPos = transform.position;

        goalPos = gotoPosition.transform.position;

        StopAllCoroutines();
        StartCoroutine(animation());

    }

    IEnumerator animation()
    {
        float t = 0f;

        while (t < timeToGetToPos)
        {
            gameObject.transform.position = Vector3.Lerp(initialPos, goalPos, t / timeToGetToPos);
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        t = 0f;

        while (t < timeToReturnToPos)
        {
            gameObject.transform.position = Vector3.Lerp(goalPos, initialPos, t / timeToReturnToPos);
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        gameObject.transform.position = initialPos;
    }
}
