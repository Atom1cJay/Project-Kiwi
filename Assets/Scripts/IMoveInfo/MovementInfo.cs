using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInfo : MonoBehaviour
{
    [HideInInspector] public float currentSpeedHoriz;
    [HideInInspector] public float currentSpeedVert;
    [HideInInspector] public int tjJumpCount;
    [SerializeField] CollisionDetector groundDetector;

    public bool IsJumpValid()
    {
        // TODO stub
        return false;
    }

    public bool isTripleJumpValid()
    {
        return false; // TODO stub
    }

    /// <summary>
    /// Determines whether the player is currently touching the ground.
    /// </summary>
    public bool touchingGround()
    {
        return groundDetector.Colliding();
    }
}
