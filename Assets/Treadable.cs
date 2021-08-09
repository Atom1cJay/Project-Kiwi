using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Treadable : MonoBehaviour
{
    [SerializeField] UnityEvent OnTread;

    /// <summary>
    /// Calls the specific event to occur when this object is treaded on.
    /// </summary>
    public void BroadcastTreadEvent()
    {
        OnTread.Invoke();
    }
}
