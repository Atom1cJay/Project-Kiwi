using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class containing useful functionality for all dialogue trees.
/// Also allows them to be serializable.
/// </summary>
public abstract class ADialogueTree : MonoBehaviour, IDialogueTree
{
    public abstract void Advance();

    public abstract void AdvanceToEnd();

    public abstract void ChooseResponse(int index);

    public abstract string[] GetResponses();

    public abstract string GetText();

    public abstract bool HasNextSegment();

    public abstract void NextSegment();

    public abstract void Reset();

    public abstract bool SegmentIsComplete();
}
