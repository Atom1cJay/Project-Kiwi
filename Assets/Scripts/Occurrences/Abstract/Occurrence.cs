using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Occurrence : MonoBehaviour
{
    public event Action OnOccurrenceStart;

    protected void InvokeOnOccurrenceStart()
    {
        OnOccurrenceStart.Invoke();
    }
}
