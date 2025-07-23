using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumRotatorMovingPlatform : AMovingPlatform
{

    [SerializeField] float pace;
    [SerializeField] Vector3 angleExtremity;
    Vector3 initRot;

    public override void Rotate()
    {
        float cosValue = Mathf.Cos(Time.time * pace);
        transform.localEulerAngles = initRot + new Vector3(cosValue * angleExtremity.x, cosValue * angleExtremity.y, cosValue * angleExtremity.z);
    }

    public override void Translate()
    {

    }

    void Start()
    {
        initRot = transform.localEulerAngles;
    }

}
