using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// NOTE: This class needs to be put on the HOLDER for whatever platform is
/// treadable. Said platform also needs to be marked as a moving platform.
/// </summary>
public class Treadable : MonoBehaviour
{
    [SerializeField] UnityEvent OnTread;

    private void FixedUpdate()
    {
        if (transform.Find("Player") != null)
        {
            BroadcastTreadEvent();
        }
    }

    /// <summary>
    /// Calls the specific event to occur when this object is treaded on.
    /// </summary>
    private void BroadcastTreadEvent()
    {
        print("fuck");
        OnTread.Invoke();
    }
}
