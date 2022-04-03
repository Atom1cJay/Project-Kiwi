using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumRotator : MonoBehaviour
{
    [SerializeField] float pace;
    [SerializeField] Vector3 angleOne;
    [SerializeField] Vector3 angleTwo;

    private void Update()
    {
        float sinValue = (Mathf.Sin(Time.time * pace) + 1) / 2;
        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(angleOne), Quaternion.Euler(angleTwo), sinValue);
    }
}
