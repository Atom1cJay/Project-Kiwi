using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject PlayingScreen, PauseScreen, MapScreen, OptionsScreen;
    [SerializeField] Camera MapCamera, PlayerCamera;
    [SerializeField] RawImage MapSpriteRenderer;
    [SerializeField] GameObject ResumeButton, PauseToOptionsButton, OptionsToPauseButton, PauseToMapButton, MapToPauseButton;
    [SerializeField] GameObject PlayerCompass;
    [SerializeField] InputActionsHolder IAH;

    [SerializeField] GameObject[] checkpoints;

    bool paused, takeScreenshotOnNextFrame;

    int width, height;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayScreen();
    }

    private void Awake()
    {
        width = MapCamera.pixelWidth;
        height = MapCamera.pixelHeight;
        if (takeScreenshotOnNextFrame && false)
        {

            MapCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
            takeScreenshotOnNextFrame = false;
            //disable fog
            //RenderSettings.fog = false;

            //RenderTexture renderTexture = MapCamera.targetTexture;
            
            Texture2D renderResult = new Texture2D(width, height, TextureFormat.ARGB32, false);
            //MapCamera.Render();
            //RenderTexture.active = MapCamera.targetTexture;
            Rect rect = new Rect(0, 0, width, height);
            renderResult.ReadPixels(rect, 0, 0);

            MapSpriteRenderer.texture = renderResult;

            RenderTexture.ReleaseTemporary(MapCamera.targetTexture);
            MapCamera.targetTexture = null;
            //RenderSettings.fog = true;
            //re enable fog

        }

    }

    void TakeScreenshot()
    {
        //takeScreenshotOnNextFrame = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!paused && (Input.GetKeyDown(KeyCode.I) || IAH.inputActions.UI.Pause.ReadValue<float>() > 0))
            SetPauseScreen();
    }

    public void SetPauseScreen()
    {
        RenderSettings.fog = true;

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

        //overhead
        PlayerCompass.SetActive(false);

        foreach (GameObject c in checkpoints)
            c.GetComponentInChildren<SpriteRenderer>().enabled = false;

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

        PlayerCompass.SetActive(false);

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

        TakeScreenshot();

        //clear selected
        EventSystem.current.SetSelectedGameObject(null);
        //new selected
        EventSystem.current.SetSelectedGameObject(MapToPauseButton);

        //set up fog and overhead
        PlayerCompass.SetActive(true);

        foreach (GameObject c in checkpoints)
            c.GetComponentInChildren<SpriteRenderer>().enabled = true;

        //render fog off
        RenderSettings.fog = false;
        MapCamera.Render();
        RenderSettings.fog = true;
        PlayerCamera.Render();


    }
}
