using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStart : MonoBehaviour, UIInterface
{

    [SerializeField] InputActionsHolder IAH;
    [SerializeField] UIControlSystem cs;
    [SerializeField] GameObject OptionsMenu;
    bool paused;

    void Start()
    {
        OptionsMenu.SetActive(true);

        //setup camera at start
        foreach (UICameraOptions c in GetComponentsInChildren<UICameraOptions>())
            c.Toggle();

        OptionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused && IAH.inputActions.UI.Pause.ReadValue<float>() > 0)
            EnableThisObject();

    }
    
    public void EnableThisObject()
    {
        paused = true;
        TimescaleHandler.setPausedForMenu(true);
        cs.OnClickFunction();

    }

    public void DisableThisObject()
    {
        paused = false;
        TimescaleHandler.setPausedForMenu(false);

    }

}
