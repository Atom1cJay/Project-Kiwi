using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnDestroy : MonoBehaviour
{
    [SerializeField] UnityEvent onDestroy;

    private void OnDestroy()
    {
        onDestroy.Invoke();
    }
}
