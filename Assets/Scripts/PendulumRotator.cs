using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumRotator : MonoBehaviour
{
    [SerializeField] float pace;
    [SerializeField] float angleExtremity;

    private void Update()
    {
        float cosValue = Mathf.Cos(Time.time * pace);
        transform.localRotation *= Quaternion.AngleAxis(cosValue * angleExtremity * Time.deltaTime, Vector3.forward/*transform.forward*/);
    }
}
