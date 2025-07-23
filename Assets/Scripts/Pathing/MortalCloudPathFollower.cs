using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalCloudPathFollower : CloudPathFollower
{
    [SerializeField] float timeToShrink;
    float timeElapsed;
    Vector3 initScale;

    private void Start()
    {
        initScale = transform.localScale;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        transform.localScale = Vector3.Lerp(initScale, Vector3.zero, timeElapsed / timeToShrink);
        if (timeElapsed > timeToShrink)
        {
            Destroy(gameObject);
        }
    }
}
