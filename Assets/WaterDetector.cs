using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterDetector : MonoBehaviour
{
    public UnityEvent OnHitWater;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            OnHitWater.Invoke();
            print("detected water");
        }
    }
}
