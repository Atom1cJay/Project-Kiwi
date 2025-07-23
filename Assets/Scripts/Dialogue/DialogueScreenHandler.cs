using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the dialogue sequence happen, either when requested by the
/// player's DialogueManager or when StartDialogueSequence is called
/// from some other script. Handles the overall dialogue sequence,
/// including making the dialogue screen appear and making its dialogue play.
/// </summary>
public class DialogueScreenHandler : MonoBehaviour
{
    [SerializeField] DialogueManager dm;
    [SerializeField] DialoguePlayer dp;
    [SerializeField] GameObject dialogueScreen;
    bool inDialogueSequence;

    void Start()
    {
        dm.OnDialogueRequested.AddListener((d) => StartDialogueSequence(d));
        dp.onDialogueEnd.AddListener(() => EndDialogueSequence());
    }

    public void StartDialogueSequence(Dialogue d)
    {
        if (!inDialogueSequence)
        {
            inDialogueSequence = true;
            dialogueScreen.SetActive(true);
            dp.PlayDialogue(d);
            TimescaleHandler.setPausedForDialogue(true);
        }
    }

    public void EndDialogueSequence()
    {
        if (inDialogueSequence)
        {
            inDialogueSequence = false;
            dialogueScreen.SetActive(false);
            TimescaleHandler.setPausedForDialogue(false);
        }
    }
}
