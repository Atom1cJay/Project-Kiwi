using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAudioController : MonoBehaviour
{

    [SerializeField] EventSystem eventSystem;
    [SerializeField] string navigateButtonsSound, startPauseMenu, closePauseMenu, selectOption;
    GameObject selected = null;
    bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

        GameObject temp = eventSystem.currentSelectedGameObject;
        

        //play nothing
        if (temp == null && selected == null)
        {

        }
        else if (clicked)
        {
            clicked = false;
        }
        //if we're going from nothing to something, play start
        else if (selected == null && temp != null)
        {
            AudioMasterController.instance.PlaySound(startPauseMenu);
        }
        //exiting
        else if (selected != null && temp == null)
        {
            AudioMasterController.instance.PlaySound(closePauseMenu);
        }
        else if (!temp.Equals(selected))
        {
            AudioMasterController.instance.PlaySound(navigateButtonsSound);
        }

        selected = temp;
    }

    public void OnClicked()
    {
        AudioMasterController.instance.PlaySound(selectOption);
        clicked = true;
    }
}
