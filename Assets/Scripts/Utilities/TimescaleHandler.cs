using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decides whether time passage in the game should be stopped or not,
/// based on whether certain game states are actvie.
/// </summary>
public class TimescaleHandler : MonoBehaviour
{
    static bool gamePausedForMenu = false;
    static bool gamePausedForCameraTransition = false;
    static bool gamePausedForDialogue = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetDomain()
    {
        gamePausedForMenu = false;
        gamePausedForCameraTransition = false;
        gamePausedForDialogue = false;
    }

    void Awake()
    {
        UpdateTimeScale();
    }

    /// <summary>
    /// Document whether or not the game is paused for the sake of the pause
    /// menu.
    /// </summary>
    public static void setPausedForMenu(bool state)
    {
        gamePausedForMenu = state;
        UpdateTimeScale();
    }

    /// <summary>
    /// Document whether or not the game is paused for some camera transition
    /// </summary>
    public static void setPausedForCameraTransition(bool state)
    {
        gamePausedForCameraTransition = state;
        UpdateTimeScale();
    }

    public static void setPausedForDialogue(bool state)
    {
        gamePausedForDialogue = state;
        UpdateTimeScale();
    }

    /// <summary>
    /// According to what is paused in the game state, determines the time scale.
    /// </summary>
    private static void UpdateTimeScale()
    {
        Time.timeScale = (gamePausedForMenu || gamePausedForCameraTransition || gamePausedForDialogue) ? 0 : 1;
    }
}
