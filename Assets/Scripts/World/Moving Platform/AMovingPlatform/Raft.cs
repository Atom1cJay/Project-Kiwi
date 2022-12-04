using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raft : MonoBehaviour
{

    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    [SerializeField] float sinkTime;// Time moving from endRisePos to startSinkPos
    [SerializeField] float pauseFloatTime; // Time to pause before sinking
    [SerializeField] float riseMult; // Rise mult

    float deltaTime;
    float relativeProgress; // From 0 (starting) to 1 (ending)

    private void Start()
    {
        deltaTime = 0f;
        GoToStartTransform();
    }

    void GoToStartTransform()
    {
        transform.position = startPos.position;
    }

    void setPosition(bool increasing = false)
    {
        float actualProgress = relativeProgress - pauseFloatTime;

        if (actualProgress < 0)
        {
            actualProgress = 0f;
        }

        float pct = Mathf.Clamp(actualProgress / sinkTime, 0f, 1f);

        float adjustedPct = increasing ? easeInBounce(pct) : pct;

        Vector3 btwnPos = Vector3.Lerp(startPos.position, endPos.position, adjustedPct);
        transform.position = btwnPos;
    }

    float easeOutExpo(float x) {
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }

    float easeInBounce(float x)
    {
        return 1 - easeOutBounce(1 - x);
    }

    float easeOutBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }

    public void Translate()
    {
        StopCoroutine("rise");

        /*
        if (deltaTime <= maxDeltaTime)
        {
            deltaTime += Time.deltaTime * acceleration;
        }*/

        relativeProgress += Time.deltaTime;

        setPosition();

        StartCoroutine("rise");
    }

    IEnumerator rise()
    {
        yield return new WaitForSeconds(Time.deltaTime);

        //If we've stopped floating, sink
        if (relativeProgress >= pauseFloatTime)
        {
            while ((relativeProgress - pauseFloatTime) < sinkTime)
            {
                relativeProgress += Time.deltaTime;
                setPosition();
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        //rise
        while (relativeProgress > 0f)
        {
            /*
            if (deltaTime >= -maxDeltaTime)
            {
                deltaTime -= Time.deltaTime * acceleration;
            }*/

            relativeProgress += -Time.deltaTime * riseMult;
            setPosition(true);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        deltaTime = 0f;
    }

}
