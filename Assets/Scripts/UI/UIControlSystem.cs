using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Utilities;

public class UIControlSystem : MonoBehaviour, UIInterface
{
    [SerializeField] List<GameObject> ObjectsToEnable;
    [SerializeField] List<GameObject> ObjectsToDisable;
    [SerializeField] GameObject ObjectToSetFirst;


    public void EnableThisObject()
    {
        gameObject.SetActive(true);


    }

    public void DisableThisObject()
    {
        gameObject.SetActive(false);

        //clear selected
        EventSystem.current.SetSelectedGameObject(null);

    }
    public void OnClickFunction()
    {
        foreach (GameObject g in ObjectsToDisable)
        {
            if (g.GetComponent<UIInterface>() != null)
                g.GetComponent<UIInterface>().DisableThisObject();
            else
                g.SetActive(false);

        }

        foreach (GameObject g in ObjectsToEnable)
        {
            if (g.GetComponent<UIInterface>() != null)
                g.GetComponent<UIInterface>().EnableThisObject();
            else
                g.SetActive(true);

        }

        //clear selected
        EventSystem.current.SetSelectedGameObject(null);

        //new selected
        if (ObjectToSetFirst != null)
            EventSystem.current.SetSelectedGameObject(ObjectToSetFirst);
    }
}
