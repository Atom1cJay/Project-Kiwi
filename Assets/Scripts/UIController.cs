using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject PlayingScreen, PauseScreen, MapScreen, OptionsScreen;
    [SerializeField] Camera MapCamera, PlayerCamera;
    [SerializeField] RawImage MapSpriteRenderer;
    [SerializeField] GameObject ResumeButton, PauseToOptionsButton, OptionsToPauseButton, PauseToMapButton, MapToPauseButton;
    [SerializeField] GameObject PlayerCompass;

    [SerializeField] Toggle ToggleX, ToggleY;

    [SerializeField] InputActionsHolder IAH;

    [SerializeField] Vector2 MapMovementSpeed;

    [SerializeField] float ZoomSpeed, mapSizeSide, mapSizeSnappingBack, camSizeSnappingBack;

    GameObject[] checkpoints;

    [SerializeField] CameraControl cc;

    bool paused, onMap, canToggleSpeedAgain, canToggleZoomAgain;

    float initialSize, speedMult, zoomMult;

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
        ToggleXFunction();
        ToggleYFunction();
        canToggleSpeedAgain = true;
        canToggleZoomAgain = true;
        speedMult = 1f;
        zoomMult = 1f;
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
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

            if (IAH.inputActions.UI.ToggleSpeed.ReadValue<float>() > 0 && canToggleSpeedAgain)
            {
                if (speedMult != 4f)
                    speedMult *= 2f;
                else
                    speedMult = 1f;

                canToggleSpeedAgain = false;

            }


            if (IAH.inputActions.UI.ToggleSpeed.ReadValue<float>() == 0)
                canToggleSpeedAgain = true;

            if (IAH.inputActions.UI.ToggleZoom.ReadValue<float>() > 0 && canToggleZoomAgain)
            {
                if (zoomMult != 4f)
                    zoomMult *= 2f;
                else
                    zoomMult = 1f;

                canToggleZoomAgain = false;

            }

            if (IAH.inputActions.UI.ToggleZoom.ReadValue<float>() == 0)
                canToggleZoomAgain = true;


            Vector2 mvt = IAH.inputActions.UI.Move.ReadValue<Vector2>();
            float zoom = IAH.inputActions.UI.Zoom.ReadValue<float>();

            Vector3 tempPos = MapCamera.transform.position + new Vector3(mvt.x * MapMovementSpeed.x, 0f, mvt.y * MapMovementSpeed.y) * IndependentTime.deltaTime * speedMult;
            float tempSize = MapCamera.orthographicSize + zoom * ZoomSpeed * IndependentTime.deltaTime * zoomMult;

            float halfCam = tempSize / 2f;

            Debug.Log("vect" + (Mathf.Abs(tempPos.x) + halfCam) + ",  other: " + (Mathf.Abs(tempPos.z) + halfCam));
            
            if ((Mathf.Abs(tempPos.x) + halfCam) < mapSizeSide && (Mathf.Abs(tempPos.z) + halfCam) < mapSizeSide)
            {
                if ((Mathf.Abs(tempPos.x) + halfCam) > mapSizeSnappingBack || (Mathf.Abs(tempPos.z) + halfCam) > mapSizeSnappingBack)
                {
                    // x
                    if (tempPos.x + halfCam > mapSizeSnappingBack)
                        tempPos += new Vector3(-MapMovementSpeed.x * IndependentTime.deltaTime, 0f, 0f);

                    //-x
                    if (Mathf.Abs(tempPos.x - halfCam) > mapSizeSnappingBack)
                        tempPos += new Vector3(MapMovementSpeed.x * IndependentTime.deltaTime, 0f, 0f);


                    // y
                    if (tempPos.z + halfCam > mapSizeSnappingBack)
                        tempPos += new Vector3(0f, 0f, -MapMovementSpeed.y * IndependentTime.deltaTime);

                    //-y
                    if (Mathf.Abs(tempPos.z - halfCam) > mapSizeSnappingBack)
                        tempPos += new Vector3(0f, 0f, MapMovementSpeed.y * IndependentTime.deltaTime);


                }
                //size
                else if (tempSize > camSizeSnappingBack)
                {
                    // x
                    if (tempPos.x > 0f)
                        tempPos += new Vector3(-MapMovementSpeed.x * IndependentTime.deltaTime, 0f, 0f) * .5f;

                    //-x
                    if (tempPos.x < 0f)
                        tempPos += new Vector3(MapMovementSpeed.x * IndependentTime.deltaTime, 0f, 0f) * .5f;


                    // y
                    if (tempPos.z > 0f)
                        tempPos += new Vector3(0f, 0f, -MapMovementSpeed.y * IndependentTime.deltaTime) * .5f;

                    //-y
                    if (tempPos.z < 0f)
                        tempPos += new Vector3(0f, 0f, MapMovementSpeed.y * IndependentTime.deltaTime) * .5f;

                    tempSize -= ZoomSpeed * 2f * IndependentTime.deltaTime;
                }

                MapCamera.transform.position = tempPos;
                MapCamera.orthographicSize = tempSize;

            }

        }
        else
        {
            speedMult = 1f;
            zoomMult = 1f;
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

    public void ToggleXFunction()
    {
        cc.ToggleX(ToggleX.isOn);
    }

    public void ToggleYFunction()
    {
        cc.ToggleY(ToggleY.isOn);
    }

}
