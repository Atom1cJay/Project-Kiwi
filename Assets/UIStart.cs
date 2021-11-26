using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStart : MonoBehaviour, UIInterface
{

    [SerializeField] InputActionsHolder IAH;
    [SerializeField] MoveExecuter me;
    [SerializeField] UIControlSystem cs;
    [SerializeField] GameObject OptionsMenu;
    bool paused;

    void Start()
    {
        //OptionsMenu.SetActive(true);

        //setup camera at start
        //foreach (UICameraOptions c in GetComponentsInChildren<UICameraOptions>())
            //c.Toggle();

        //OptionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused && IAH.inputActions.UI.Pause.ReadValue<float>() > 0 && 
            (me.GetCurrentMove().GetHorizSpeedThisFrame().magnitude == 0f && me.GetCurrentMove().GetVertSpeedThisFrame() == 0))
            EnableThisObject();

    }
    
    public void EnableThisObject()
    {
        paused = true;
       MonobehaviourUtils.Instance.StartCoroutine(TimeScaleHandler(true));
        cs.OnClickFunction();

    }

    public void DisableThisObject()
    {
        paused = false;
        StartCoroutine(TimeScaleHandler(false));

    }

    IEnumerator TimeScaleHandler(bool b)
    {
        yield return new WaitForSecondsRealtime(0f);
        TimescaleHandler.setPausedForMenu(b);
    }

}
