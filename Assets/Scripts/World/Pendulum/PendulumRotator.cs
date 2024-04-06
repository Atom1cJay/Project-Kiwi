using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumRotator : MonoBehaviour
{
    [SerializeField] float pace;
    [SerializeField] float angleExtremity;
    Quaternion initRot;

    void Start()
    {
        initRot = transform.localRotation;
    }

    void Update()
    {
        float cosValue = Mathf.Cos(Time.time * pace);
        transform.localRotation = initRot * Quaternion.AngleAxis(cosValue * angleExtremity, Vector3.forward);
    }
}
