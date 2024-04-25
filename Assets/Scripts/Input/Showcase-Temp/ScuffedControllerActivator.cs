using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScuffedControllerActivator : MonoBehaviour
{
    [SerializeField] UnityEvent onKeyboardSet;
    [SerializeField] UnityEvent onXboxSet;
    [SerializeField] UnityEvent onGamecubeSet;
    [SerializeField] Toggle keyboardToggle;
    [SerializeField] Toggle xboxToggle;
    [SerializeField] Toggle gamecubeToggle;

    void Start()
    {
        if (PlayerPrefs.HasKey("ControllerDemoChoice"))
        {
            SetController(PlayerPrefs.GetString("ControllerDemoChoice"));
            SetToggle(PlayerPrefs.GetString("ControllerDemoChoice"));
        }
        else
        {
            SetController("keyboard");
            SetToggle("keyboard");
        }
    }

    public void SetController(string type)
    {
        Debug.Log(type);
        if (type == "keyboard")
        {
            PlayerPrefs.SetString("ControllerDemoChoice", "keyboard");
            onKeyboardSet.Invoke();
        }
        else if (type == "xbox")
        {
            PlayerPrefs.SetString("ControllerDemoChoice", "xbox");
            onXboxSet.Invoke();
        }
        else
        {
            PlayerPrefs.SetString("ControllerDemoChoice", "gamecube");
            onGamecubeSet.Invoke();
        }
    }

    public void SetToggle(string type)
    {
        Debug.Log(type);
        if (type == "keyboard")
        {
            keyboardToggle.isOn = true;
        }
        else if (type == "xbox")
        {
            xboxToggle.isOn = true;
        }
        else
        {
            gamecubeToggle.isOn = true;
        }
    }
}
