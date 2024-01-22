using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Functionality for any object that can be landed on, and do something.
/// 
public class Landable : MonoBehaviour
{
    [SerializeField] UnityEvent OnLand;
    [SerializeField] bool oneTimeActivationPerScene;
    bool poundedThisScene = false;

    /// <summary>
    /// Calls the specific event to occur when this object is landed on.
    /// This method is called in any move when you've just landed.
    /// </summary>
    public void BroadcastLandEvent()
    {
        if (oneTimeActivationPerScene && !poundedThisScene)
        {
            return;
        }
        OnLand.Invoke();
        poundedThisScene = true;
    }
}
