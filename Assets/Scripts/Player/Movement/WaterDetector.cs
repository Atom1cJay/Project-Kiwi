using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterDetector : MonoBehaviour
{
    [SerializeField] MovementInfo mi;

    public bool isCollidingWithWater()
    {
        return mi.GetGroundDetector().Colliding()
            && mi.GetGroundDetector().CollidingWith().layer == 4; // Water layer
    }
}
