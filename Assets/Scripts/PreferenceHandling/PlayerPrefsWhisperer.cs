using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The "middleman" between PlayerPrefs and anything that uses PlayerPrefs.
/// Any changes to PlayerPrefs settings, or any accessing of PlayerPrefs settings,
/// would be best done through this script.
/// </summary>
public class PlayerPrefsWhisperer : MonoBehaviour
{
    // For when camera inversion is toggled for either X or Y
    public static UnityEvent OnCameraInversionToggled = new UnityEvent();
    public static UnityEvent<float> OnCameraSensitivityChanged = new UnityEvent<float>();

    /// <summary>
    /// Retrieves the bool value (actually stored as an int) for the given key.
    /// 0 represents false, 1 represents true, and everything else is an
    /// undefined/error value.
    /// </summary>
    static bool GetBoolValue(string key)
    {
        switch (PlayerPrefs.GetInt(key))
        {
            case 0:
                return false;
            case 1:
                return true;
            default:
                Debug.LogError("Undefined value for " + key + " in PlayerPrefs; not 0 or 1.");
                return false;
        }
    }

    /// <summary>
    /// Sets the bool value (really stored as an int) for the given key. 0 represents
    /// false and 1 represents true.
    /// </summary>
    static void SetBoolValue(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    /// <summary>
    /// Gets the camera X inversion value from PlayerPrefs. Values other than
    /// 0 (false) or 1 (true) are undefined, and will cause an error.
    /// </summary>
    public static bool GetCameraXInverted()
    {
        return GetBoolValue("CameraXInverted");
    }

    /// <summary>
    /// Gets the camera X inversion value from PlayerPrefs. Values other than
    /// 0 (false) or 1 (true) are undefined, and will cause an error.
    /// </summary>
    public static bool GetCameraYInverted()
    {
        return GetBoolValue("CameraYInverted");
    }

    /// <summary>
    /// Tells PlayerPrefs to register the camera X inversion value
    /// as either true or false, depending on given bool
    /// </summary>
    public static void SetCameraXInverted(bool inverted)
    {
        SetBoolValue("CameraXInverted", inverted);
        OnCameraInversionToggled.Invoke();
    }

    /// <summary>
    /// Tells PlayerPrefs to register the camera Y inversion value
    /// as either true or false, depending on given bool
    /// </summary>
    public static void SetCameraYInverted(bool inverted)
    {
        SetBoolValue("CameraYInverted", inverted);
        OnCameraInversionToggled.Invoke();
    }

    /// <summary>
    /// Tells PlayerPrefs to register the camera sensitivity value as the given
    /// </summary>
    public static void SetMouseSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
        OnCameraSensitivityChanged.Invoke(sensitivity);
    }

    /// <summary>
    /// Gets the CameraSensitivity value in PlayerPrefs
    /// </summary>
    public static float GetMouseSensitivity()
    {
        return PlayerPrefs.GetFloat("MouseSensitivity", 1);
    }
}
