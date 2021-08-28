using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject PlayingScreen, PauseScreen, MapScreen, OptionsScreen;
    [SerializeField] Camera PlayerCamera, MapCamera;
    [SerializeField] GameObject ResumeButton, PauseToOptionsButton, OptionsToPauseButton, PauseToMapButton, MapToPauseButton;
    [SerializeField] InputActionsHolder IAH;

    bool paused;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayScreen();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!paused && (Input.GetKeyDown(KeyCode.I) || IAH.inputActions.UI.Pause.ReadValue<float>() > 0))
            SetPauseScreen();
    }

    public void SetPauseScreen()
    {
        //set paused
        paused = true;

        //set time to 0
        TimescaleHandler.setPausedForMenu(true);

        //set screens
        PlayingScreen.SetActive(false);
        OptionsScreen.SetActive(false);
        MapScreen.SetActive(false);
        PauseScreen.SetActive(true);

        //clear selected
        EventSystem.current.SetSelectedGameObject(null);
        //new selected
        EventSystem.current.SetSelectedGameObject(ResumeButton);

    }


    public void SetPlayScreen()
    {
        //resume time
        TimescaleHandler.setPausedForMenu(false);

        //reset paused
        paused = false;

        //set screens
        PlayingScreen.SetActive(true);
        OptionsScreen.SetActive(false);
        MapScreen.SetActive(false);
        PauseScreen.SetActive(false);

        //clear selected
        EventSystem.current.SetSelectedGameObject(null);

    }

    public void SetOptionScreen()
    {

        //set screens
        PlayingScreen.SetActive(false);
        OptionsScreen.SetActive(true);
        MapScreen.SetActive(false);
        PauseScreen.SetActive(false);


        //clear selected
        EventSystem.current.SetSelectedGameObject(null);
        //new selected
        EventSystem.current.SetSelectedGameObject(OptionsToPauseButton);

    }


    public void SetMapScreen()
    {
        //set screens
        PlayingScreen.SetActive(false);
        OptionsScreen.SetActive(false);
        MapScreen.SetActive(true);
        PauseScreen.SetActive(false);


        //clear selected
        EventSystem.current.SetSelectedGameObject(null);
        //new selected
        EventSystem.current.SetSelectedGameObject(MapToPauseButton);


    }
}
