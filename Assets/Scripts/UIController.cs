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

    [SerializeField] Vector2 MapMovementSpeed;

    [SerializeField] float ZoomSpeed;

    [SerializeField] GameObject[] checkpoints;

    bool paused, onMap;

    float initialSize;

    GameObject player;

    int width, height;

    Vector3 initialMapPos;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayScreen();
    }

    private void Awake()
    {
        initialMapPos = MapCamera.transform.localPosition;
        player = MapCamera.transform.parent.gameObject;
        MapCamera.transform.SetParent(null);
        initialSize = MapCamera.orthographicSize;
    }


    // Update is called once per frame
    void Update()
    {
        if (!paused && IAH.inputActions.UI.Pause.ReadValue<float>() > 0)
            SetPauseScreen();

        
        if (onMap)
        {

            RenderSettings.fog = false;
            MapCamera.Render();

            if (IAH.inputActions.UI.Back.ReadValue<float>() > 0 || Input.GetKey(KeyCode.K))
                SetPauseScreen();

            
            Vector2 mvt = IAH.inputActions.UI.Move.ReadValue<Vector2>();
            float zoom = IAH.inputActions.UI.Zoom.ReadValue<float>();
            Debug.Log("vect" + mvt + ",  other: " + zoom);
            MapCamera.transform.position += new Vector3(mvt.x * MapMovementSpeed.x, 0f, mvt.y * MapMovementSpeed.y) * IndependentTime.deltaTime;
            MapCamera.orthographicSize += zoom * ZoomSpeed * IndependentTime.deltaTime;
        }
        else
        {
            RenderSettings.fog = true;
            MapCamera.transform.localPosition = initialMapPos;
            MapCamera.orthographicSize = initialSize;
        }
    }

    public void SetPauseScreen()
    {
        //fog
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


        //disable cam and no map
        MapCamera.enabled = false;
        onMap = false;

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


        //initialize
        RenderSettings.fog = true;
        onMap = false;
        PlayerCompass.SetActive(false);

        MapCamera.enabled = false;


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
        //enable cam
        MapCamera.enabled = false;

        MapCamera.transform.position = player.transform.position + initialMapPos;
        //set screens
        PlayingScreen.SetActive(false);
        OptionsScreen.SetActive(false);
        MapScreen.SetActive(true);
        PauseScreen.SetActive(false);

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

        onMap = true;

    }
}
