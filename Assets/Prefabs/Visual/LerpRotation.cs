using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpRotation : MonoBehaviour
{
    public Vector3 amplitudeRotations;
    public float speed;

    Vector3 initialAngles;

    // Start is called before the first frame update
    void Start()
    {
        initialAngles = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = amplitudeRotations * Mathf.Sin(Time.time * speed) + initialAngles;
    }
}
