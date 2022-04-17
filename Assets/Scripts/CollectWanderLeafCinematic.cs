using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectWanderLeafCinematic : MonoBehaviour
{
    [SerializeField] float nonCinematicScale, cinematicScale;
    [SerializeField] GameObject cinematicBorder;
    [SerializeField] TMP_Text text;

    float enterDuration, exitDuration;
    bool inCinematic, transitioning;

    // Start is called before the first frame update
    void Start()
    {
        transitioning = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (transitioning)
        {
            if (inCinematic)
            {
                float speed = ((cinematicScale - nonCinematicScale) / enterDuration) * Time.deltaTime;

                cinematicBorder.transform.localScale += new Vector3(0f, speed, 0f);

                Color c = text.color;
                text.color = new Color(c.r, c.g, c.b, c.a + (1f / enterDuration) * Time.deltaTime);

            }
            else
            {
                float speed = ((nonCinematicScale - cinematicScale) / exitDuration) * Time.deltaTime;

                cinematicBorder.transform.localScale += new Vector3(0f, speed, 0f);

                Color c = text.color;
                text.color = new Color(c.r, c.g, c.b, c.a - (1f / exitDuration) * Time.deltaTime);

            }
        }
        else
        {
            if (inCinematic)
            {
                cinematicBorder.transform.localScale = new Vector3(1f, cinematicScale, 1f);
                Color c = text.color;
                text.color = new Color(c.r, c.g, c.b, 1f);
            }
            else
            {
                cinematicBorder.transform.localScale = new Vector3(1f, nonCinematicScale, 1f);
                Color c = text.color;
                text.color = new Color(c.r, c.g, c.b, 0f);
            }
        }
    }

    public void enterCinematic(float timeToEnter)
    {
        enterDuration = timeToEnter;
        inCinematic = true;
        transitioning = true;
        Invoke("doneTransitioning", timeToEnter);

    }

    void doneTransitioning()
    {
        transitioning = false;
    }

    public void exitCinematic(float timeToExit)
    {
        exitDuration = timeToExit;
        inCinematic = false;
        transitioning = true;
        Invoke("doneTransitioning", timeToExit);
    }
}
