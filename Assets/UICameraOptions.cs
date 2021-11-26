using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICameraOptions : MonoBehaviour
{
    // Is this gameobject the toggle for x inversion / y inversion?
    // (Toggle component assumed to exist if either are true)
    [SerializeField] bool controlsXInversion, controlsYInversion;

    private void Start()
    {
        UpdateVisualsToPlayerPrefs();
    }

    /// <summary>
    /// Makes this object's toggle visually agree with its assigned PlayerPrefs
    /// setting.
    /// </summary>
    void UpdateVisualsToPlayerPrefs()
    {
        if (controlsXInversion)
        {
            GetComponent<Toggle>().isOn = PlayerPrefsWhisperer.GetCameraXInverted();
        }
        if (controlsYInversion)
        {
            GetComponent<Toggle>().isOn = PlayerPrefsWhisperer.GetCameraYInverted();
        }
    }

    /// <summary>
    /// To be called when the toggle for x inversion is clicked.
    /// </summary>
    public void OnXInversionToggled(bool inverted)
    {
        PlayerPrefsWhisperer.SetCameraXInverted(inverted);
    }

    /// <summary>
    /// To be called when the toggle for y inversion is clicked.
    /// </summary>
    public void OnYInversionToggled(bool inverted)
    {
        PlayerPrefsWhisperer.SetCameraYInverted(inverted);
    }
}

