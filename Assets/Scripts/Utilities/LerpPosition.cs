using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPosition : MonoBehaviour
{
    [SerializeField] Vector3 positionAmplitude;
    [SerializeField] float speed;
    Vector3 initialPos;

    private void Awake()
    {
        initialPos = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = initialPos + positionAmplitude * Mathf.Sin(Time.time * speed);
    }
}
