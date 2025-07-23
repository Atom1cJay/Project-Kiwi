using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWheelPathFollower : PathFollower
{
    [SerializeField] Vector3 extraRotation;
    Vector3 savedEulers;

    public override void Translate()
    {
        base.Translate();
        if (!rotateWithPath)
        {
            transform.Rotate(extraRotation * Time.deltaTime);
        }
        else
        {
            savedEulers += extraRotation * Time.deltaTime;
            transform.Rotate(savedEulers);
        }
    }
}
