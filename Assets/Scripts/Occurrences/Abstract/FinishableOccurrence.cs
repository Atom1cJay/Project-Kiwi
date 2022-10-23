using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FinishableOccurrence : Occurrence
{
    public event Action OnOccurrenceFinish;

    protected void InvokeOnOccurrenceFinish()
    {
        OnOccurrenceFinish.Invoke();
    }
}
