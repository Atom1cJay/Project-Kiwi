using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Treadable : MonoBehaviour
{
    [Header("This GameObject Must Be A Moving Platform.")]
    [SerializeField] UnityEvent OnTread;

    private void FixedUpdate()
    {
        if (transform.parent.Find("Player") != null)
        {
            BroadcastTreadEvent();
        }
    }

    /// <summary>
    /// Calls the specific event to occur when this object is treaded on.
    /// </summary>
    private void BroadcastTreadEvent()
    {
        OnTread.Invoke();
    }
}
