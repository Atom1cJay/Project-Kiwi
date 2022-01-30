using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// UI object that is capable of changing scenes, and activating transition
// UI as this happens.
public class UISceneController : MonoBehaviour, UIInterface
{
    [SerializeField] string scene; // Scene to change to
    [SerializeField] GameObject transitionScreen; // UI to appear on transition (assumed inactive until pushed)
    [SerializeField] float sceneSwitchTimer; // Time before scene switch

    public void DisableThisObject()
    {
        // Nothing
    }

    public void EnableThisObject()
    {
        StartCoroutine(NextSceneAnimation());
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator NextSceneAnimation()
    {
        EventSystem.current.enabled = false; // We're disabling any further user input!
        transitionScreen.SetActive(true);
        yield return new WaitForSeconds(sceneSwitchTimer);
        SceneManager.LoadScene(scene);
    }
}
