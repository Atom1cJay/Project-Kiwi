using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLeaf : ADialogueTree
{
    public override void Advance()
    {
        // Nothing
    }

    public override void AdvanceToEnd()
    {
        // Nothing
    }

    public override void ChooseResponse(int index)
    {
        // Nothing
    }

    public override string[] GetResponses()
    {
        return null;
    }

    public override string GetText()
    {
        // Signal Error
        Debug.LogError("GetText() called on a dialogue leaf. This may be a mistake.");
        return "";
    }

    public override bool HasNextSegment()
    {
        return false;
    }

    public override void NextSegment()
    {
        // Nothing
    }

    public override void Reset()
    {
        // Nothing
    }

    public override bool SegmentIsComplete()
    {
        return true;
    }
}
