using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raft : MonoBehaviour
{

    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;

    [SerializeField] float pauseFloatTime; // Time to pause before sinking
    [SerializeField] float sinkTime;// Time moving from endRisePos to startSinkPos
    [SerializeField] float riseTime; // time to rise
    [SerializeField] float pauseAfterSinkTime; 
    [SerializeField] float pauseAfterWobbleTime; // time to rise
    [SerializeField] float wobbleTime = 0.5f;
    [SerializeField] float wobbleAmplitude = 20f;

    float relativeProgress; // From 0 (starting) to 1 (ending)

    Vector3 defaultAngle;

    bool translating = false;
    bool sinking = false;

    private void Start()
    {
        defaultAngle = transform.localEulerAngles;
        GoToStartTransform();
    }

    void GoToStartTransform()
    {
        transform.position = startPos.position;
    }

    void setPosition(float pct, bool increasing)
    {
        float adjustedPct = increasing ? easeInBounce(pct) : pct;

        Vector3 goalPos = Vector3.Lerp(startPos.position, endPos.position, adjustedPct);
        transform.position = goalPos;
    }
    void wobbleEffect(float pct)
    {
        float goalXAngle = defaultAngle.x + Mathf.Sin(pct * 2 * Mathf.PI) * wobbleAmplitude;

        Vector3 goalAngle = new Vector3(goalXAngle, defaultAngle.y, defaultAngle.z);

        transform.localEulerAngles = goalAngle;
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
        if (!sinking)
        {
            StopCoroutine("offRaft");

            translating = true;

            if (relativeProgress >= pauseFloatTime)
            {
                translating = false;
                sinking = true;

                StartCoroutine("sinkRaft");
            }
            else
            {
                StartCoroutine("offRaft");
            }

        }
    }

    IEnumerator offRaft()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        translating = false;
    }

    private void Update()
    {
        if (!sinking)
        {
            if (translating)
            {
                relativeProgress += Time.deltaTime;
            }
            else
            {
                float tempProgress = relativeProgress - Time.deltaTime;

                relativeProgress = tempProgress < 0 ? 0f : tempProgress;
            }
        }
    }

    IEnumerator sinkRaft()
    {
        float timeWobbling = 0f;

        while (timeWobbling <= wobbleTime)
        {
            timeWobbling += Time.deltaTime;
            wobbleEffect(timeWobbling / wobbleTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        float pauseWobblingTime = 0f;

        while (pauseWobblingTime <= pauseAfterWobbleTime)
        {
            pauseWobblingTime += Time.deltaTime;
            wobbleEffect(1f);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(pauseAfterWobbleTime);

        float timeSinking = 0f;

        while (timeSinking < sinkTime)
        {
            timeSinking += Time.deltaTime;
            setPosition(timeSinking / sinkTime, false);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(pauseAfterSinkTime);

        float timeRising = 0f;

        while (timeRising < riseTime)
        {
            timeRising += Time.deltaTime;
            setPosition(1f - (timeRising / riseTime), true);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        setPosition(0f, true);

        sinking = false;
        relativeProgress = 0f;
    }

}
