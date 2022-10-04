using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRotatingPlatform : AMovingPlatform
{
    [SerializeField] Vector3 rotSpeed;
    public override void Rotate()
    {
        transform.localEulerAngles += rotSpeed * Time.deltaTime;
    }

    public override void Translate()
    {

    }
}
