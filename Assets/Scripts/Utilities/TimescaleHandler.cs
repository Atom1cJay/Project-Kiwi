using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimescaleHandler : MonoBehaviour
{
    static bool gamePausedForMenu = false;
    static bool gamePausedForCameraTransition = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetDomain()
    {
        gamePausedForMenu = false;
        gamePausedForCameraTransition = false;
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

    /// <summary>
    /// According to what is paused in the game state, determines the time scale.
    /// </summary>
    private static void UpdateTimeScale()
    {
        Time.timeScale = (gamePausedForMenu || gamePausedForCameraTransition) ? 0 : 1;
    }
}
