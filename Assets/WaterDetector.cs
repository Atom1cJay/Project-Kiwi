using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterDetector : MonoBehaviour
{
    [SerializeField] MovementInfo mi;
    public UnityEvent OnHitWater;

    private void OnCollisionEnter(Collision collision)
    {
        // The water will collide with the Ground Detector. The ground
        // detector itself should not register it as a collision (as to prevent
        // any moves other than Swim from taking place).
        OnHitWater.Invoke();
    }
}
