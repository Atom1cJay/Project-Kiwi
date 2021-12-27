using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script to be held by the player. When the player's dialogue detector
/// collides with some object with dialogue, makes the prompt show up.
/// Additionally, while a prompt is active, allows the player to initiate
/// a dialogue sequence.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    Dialogue activeDialogue; // Either the dialogue being prompted or the dialogue being read
    [SerializeField] CollisionDetector dialogueCollisionDetector;
    [SerializeField] InputActionsHolder iah;
    [SerializeField] GameObject promptSprite; // There is only one prompt sprite in the game, and it is either inactive or over a specific dialogue holding gameObject.
    public DialogueEvent OnDialogueRequested = new DialogueEvent();

    private void Start()
    {
        iah.inputActions.Player.Dialogue.performed += _ => PlayDialogue();
    }

    /// <summary>
    /// Has a dialogue take place, if there is an active dialogue prompt.
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
    /// Update whether a dialogue prompt is active or not,
    /// and update the placement of the prompt sprite.
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
