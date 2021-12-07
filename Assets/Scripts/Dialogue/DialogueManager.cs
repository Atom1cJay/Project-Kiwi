using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages which dialogue instance is "active" / being interacted with. Requests that
/// the dialogue be played at the player's command.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    Dialogue activeDialogue; // Either the dialogue being prompted or the dialogue being read
    [SerializeField] CollisionDetector dialogueCollisionDetector;
    [SerializeField] InputActionsHolder iah;
    [SerializeField] GameObject promptSprite;
    public DialogueEvent OnDialogueRequested = new DialogueEvent();

    private void Start()
    {
        iah.inputActions.Player.Dialogue.performed += _ => PlayDialogue();
    }

    /// <summary>
    /// Has a dialogue take place in a frozen world, if there is an active dialogue.
    /// </summary>
    void PlayDialogue()
    {
        if (activeDialogue != null)
        {
            promptSprite.SetActive(false);
            OnDialogueRequested.Invoke(activeDialogue);
        }
    }

    /// <summary>
    /// Update whether a dialogue is active or not, and update the placement of the prompt sprite.
    /// </summary>
    private void Update()
    {
        GameObject d = dialogueCollisionDetector.CollidingWith(); // d should have dialogue component if in correct layer
        if (d != null)
        {
            Dialogue dComponent = d.GetComponent<Dialogue>();
            if (dComponent != activeDialogue)
            {
                promptSprite.transform.position = dComponent.GetPromptTransform().position;
                promptSprite.SetActive(true);
                activeDialogue = dComponent;
            }
        }
        else
        {
            activeDialogue = null;
            promptSprite.SetActive(false);
        }
    }
}
