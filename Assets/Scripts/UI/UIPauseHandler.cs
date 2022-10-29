using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Allows pausing to take place. Doesn't handle anything related to menus,
/// just pauses and unpauses the flow of the game.
/// </summary>
public class UIPauseHandler : MonoBehaviour
{
    [SerializeField] InputActionsHolder IAH;
    [SerializeField] MoveExecuter me;
    public UnityEvent onPaused;
    bool paused;

    void Start()
    {
        IAH.inputActions.UI.Pause.performed += _ => ConsiderPausing();
    }

    /// <summary>
    /// Makes the game pause if it is in the proper state for that to happen
    /// (the game is not already paused,
    /// </summary>
    void ConsiderPausing()
    {
        if (!paused && IAH.inputActions.UI.Pause.ReadValue<float>() > 0 && MoveExecuter.instance.GetCurrentMove().Pausable())
            ForcePause();
    }

    /// <summary>
    /// Pauses the game, regardless of the current game status.
    /// </summary>
    public void ForcePause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        paused = true;
        TimescaleHandler.setPausedForMenu(true);
        onPaused.Invoke();
    }

    /// <summary>
    /// Pauses the game, regardless of the current game status.
    /// </summary>
    public void ForceUnpause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        paused = false;
        TimescaleHandler.setPausedForMenu(false);
    }
}
