using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Contains information related to a particular dialogue sequence.
/// </summary>
[System.Serializable]
public class Dialogue : MonoBehaviour
{
    [SerializeField] Transform promptAppearTransform;
    [SerializeField] ADialogueTree dialogueTree;

    public IDialogueTree GetDialogueTree()
    {
        return dialogueTree;
    }

    public Transform GetPromptTransform()
    {
        return promptAppearTransform;
    }
}
