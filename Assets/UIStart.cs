using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStart : MonoBehaviour, UIInterface
{
    [SerializeField] InputActionsHolder IAH;
    [SerializeField] MoveExecuter me;
    [SerializeField] UIControlSystem cs;
    bool paused;

    void Update()
    {
        if (!paused && IAH.inputActions.UI.Pause.ReadValue<float>() > 0 && me.GetCurrentMove().Pausable())
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
