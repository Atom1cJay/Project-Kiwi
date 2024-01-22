using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpScaler : MonoBehaviour
{
    [SerializeField] Vector3 initialScaleGoal, goalScale;
    [SerializeField] float speed;

    bool hitInitialGoal = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 actualGoal = Vector3.Slerp(initialScaleGoal, goalScale, (Mathf.Cos(Time.time * speed) + 1f) / 2f);

        if (!hitInitialGoal)
        {
            transform.localScale = Vector3.Slerp(transform.localScale, actualGoal, Time.deltaTime * speed);

            if (Vector3.Distance(transform.localScale, actualGoal) <= 0.001f)
                hitInitialGoal = true;
        }
        else
        {
            transform.localScale = actualGoal;
        }

    }
}
