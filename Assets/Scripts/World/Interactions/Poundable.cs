using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Functionality for any object that can be ground pounded.
/// 
public class Poundable : MonoBehaviour
{
    [SerializeField] UnityEvent OnPound;

    /// <summary>
    /// Calls the specific event to occur when this object is pounded.
    /// This method is called within the ground pound move.
    /// </summary>
    public void BroadcastPoundEvent()
    {
        OnPound.Invoke();
    }
}
