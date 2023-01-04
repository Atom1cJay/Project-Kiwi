using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Treadable : MonoBehaviour
{
    [SerializeField] UnityEvent OnTread;
    [SerializeField] bool oneTimeActivationPerScene;
    bool cannotActivate;

    private void OnCollisionStay(Collision collision)
    {
        if (cannotActivate)
        {
            return;
        }
        if (collision.gameObject.layer == 9 && MovementInfo.instance.GetGroundDetector().CollidingWith() == gameObject)
        {
            BroadcastTreadEvent();
        }
    }

    /// <summary>
    /// Calls the specific event to occur when this object is treaded on.
    /// </summary>
    private void BroadcastTreadEvent()
    {
        if (oneTimeActivationPerScene)
        {
            cannotActivate = true;
        }
        //print("Tread Event");
        OnTread.Invoke();
    }
}
