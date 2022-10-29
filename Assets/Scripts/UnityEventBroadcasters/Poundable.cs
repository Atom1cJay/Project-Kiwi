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
    [SerializeField] bool oneTimeActivationPerScene;
    bool poundedThisScene = false;

    /// <summary>
    /// Calls the specific event to occur when this object is pounded.
    /// This method is called within the ground pound move.
    /// </summary>
    public void BroadcastPoundEvent()
    {
        if (oneTimeActivationPerScene && !poundedThisScene)
        {
            return;
        }
        OnPound.Invoke();
        poundedThisScene = true;
    }
}
