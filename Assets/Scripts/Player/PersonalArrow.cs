using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalArrow : MonoBehaviour
{
    [SerializeField] GameObject arrowGeometry;
    Transform pointTo;
    bool showing;
    float showingTimeElapsed;
    float showingTimeLimit;

    void Start()
    {
        arrowGeometry.SetActive(false);
    }

    public void ShowArrow(Transform pointTo, float timeLimit=0) // 0 = no time limit
    {
        this.pointTo = pointTo;
        showingTimeLimit = (timeLimit == 0) ? float.MaxValue : timeLimit;
        arrowGeometry.SetActive(true);
        showingTimeElapsed = 0;
        showing = true;
    }

    public void HideArrow()
    {
        arrowGeometry.SetActive(false);
        showing = false;
    }

    void Update()
    {
        if (!showing) return;
        if (!pointTo.gameObject.activeSelf) // Assume object is hidden or no longer relevant
        {
            HideArrow();
        }
        else
        {
            arrowGeometry.transform.LookAt(pointTo);
            showingTimeElapsed += Time.deltaTime;
            if (showingTimeElapsed > showingTimeLimit)
            {
                HideArrow();
            }
        }
    }
}
