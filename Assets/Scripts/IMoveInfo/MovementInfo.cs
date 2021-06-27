using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInfo : MonoBehaviour
{
    [HideInInspector] public float currentSpeedHoriz;
    [HideInInspector] public float currentSpeedVert;
    [HideInInspector] public int tjJumpCount;

    public bool IsOnGround { get; private set; }

    public bool IsJumpValid()
    {
        // TODO stub
        return false;
    }

    public bool isTripleJumpValid()
    {
        return false; // TODO stub
    }
}
