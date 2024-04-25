using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScuffedControllerActivator : MonoBehaviour
{
    [SerializeField] UnityEvent onKeyboardSet;
    [SerializeField] UnityEvent onXboxSet;
    [SerializeField] UnityEvent onGamecubeSet;

    public void SetController(string type)
    {
        if (type == "keyboard")
        {
            onKeyboardSet.Invoke();
        }
        else if (type == "xbox")
        {
            onXboxSet.Invoke();
        }
        else
        {
            onGamecubeSet.Invoke();
        }
    }
}
