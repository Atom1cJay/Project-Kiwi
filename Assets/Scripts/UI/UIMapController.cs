using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UIMapController : MonoBehaviour, UIInterface
{
    [SerializeField] Camera MapCamera;
    [SerializeField] GameObject PlayerCompass;
    [SerializeField] InputActionsHolder IAH;
    [SerializeField] float T1MoveSpeed, T2MoveSpeed, T3MoveSpeed;
    [SerializeField] float T1ZoomSize, T2ZoomSize, T3ZoomSize;

    [SerializeField] Vector2 mapSize;

    GameObject[] checkpoints;

    float initialSize, MoveSpeed, tempSize, halfCam;

    int i;

    GameObject player;

    Vector3 initialMapPos;
    Quaternion initialRotation;

    bool paused, onMap, canToggleSpeedAgain, canToggleZoomAgain;
    // Start is called before the first frame update
    void Awake()
    {
        //initialize map movement variables
        i = 0;
        tempSize = T1ZoomSize;
        MoveSpeed = T1MoveSpeed;
        halfCam = tempSize / 2f;

        canToggleSpeedAgain = true;
        canToggleZoomAgain = true;

        onMap = false;

        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");

        //set up positions and parents
        initialMapPos = MapCamera.transform.localPosition;
        initialRotation = MapCamera.transform.localRotation;
        player = MapCamera.transform.parent.gameObject;
        MapCamera.transform.SetParent(null);
    }

    private void Update()
    {
        if (onMap)
        {
            //Fix rotation
            MapCamera.transform.rotation = initialRotation;

            //represents how far away from center the range of the cam is
            float x, y;

            //render shit
            RenderSettings.fog = false;
            MapCamera.Render();
            RenderSettings.fog = true;


            Vector3 tempPos;

            //if toggling
            if (IAH.inputActions.UI.ToggleSpeed.ReadValue<float>() > 0 && canToggleSpeedAgain)
            {
                //rotate through
                if (i < 2)
                    i += 1;
                else
                    i = 0;

                canToggleSpeedAgain = false;

                //set up vars
                if (i == 0)
                {
                    MapCamera.transform.position = player.transform.position + initialMapPos;
                    tempSize = T1ZoomSize;
                    MoveSpeed = T1MoveSpeed;
                }
                else if (i == 1)
                {
                    tempSize = T2ZoomSize;
                    MoveSpeed = T2MoveSpeed;
                }
                else
                {
                    tempSize = T3ZoomSize;
                    MoveSpeed = T3MoveSpeed;
                }

                halfCam = tempSize / 2f;

                tempPos = MapCamera.transform.position;

                x = Mathf.Abs(tempPos.x) + halfCam;
                y = Mathf.Abs(tempPos.z) + halfCam;

                //if out of range, correct
                if (x >= mapSize.x || y >= mapSize.y)
                {
                    Vector3 targetVector = -tempPos;
                    targetVector = new Vector3(targetVector.x, 0f, targetVector.z);

                    float mult = 0f;
                    while (mapSize != Vector2.zero && (x >= mapSize.x || y >= mapSize.y))
                    {
                        mult += .25f;
                        tempPos += targetVector * mult;
                        x = Mathf.Abs(tempPos.x) + halfCam;
                        y = Mathf.Abs(tempPos.z) + halfCam;
                    }
                    mult = 0f;
                    MapCamera.transform.position = tempPos;
                    MapCamera.orthographicSize = tempSize;

                }

                MapCamera.orthographicSize = tempSize;

            }

            //can toggle again
            if (IAH.inputActions.UI.ToggleSpeed.ReadValue<float>() == 0)
                canToggleSpeedAgain = true;


            Vector2 mvt = IAH.inputActions.UI.Move.ReadValue<Vector2>();
            float zoom = IAH.inputActions.UI.Zoom.ReadValue<float>();

            tempPos = MapCamera.transform.position + new Vector3(mvt.x * MoveSpeed, 0f, mvt.y * MoveSpeed) * IndependentTime.deltaTime;

            Debug.Log("vect" + (Mathf.Abs(tempPos.x) + halfCam) + ",  other: " + (Mathf.Abs(tempPos.z) + halfCam));
            x = Mathf.Abs(tempPos.x) + halfCam;
            y = Mathf.Abs(tempPos.z) + halfCam;

            Vector3 p = MapCamera.transform.position;
            //mvmt check
            if (x < mapSize.x)
                MapCamera.transform.position = new Vector3(tempPos.x, p.y, p.z);
            
            p = MapCamera.transform.position;

            if (y < mapSize.y)
                MapCamera.transform.position = new Vector3(p.x, p.y, tempPos.z);

        }
    }

    public void EnableThisObject()
    {
        onMap = true;

        //set up cam
        MapCamera.enabled = false;

        MapCamera.transform.position = player.transform.position + initialMapPos;

        //set up fog and overhead
        PlayerCompass.SetActive(true);

        foreach (GameObject c in checkpoints)
            c.GetComponentInChildren<SpriteRenderer>().enabled = true;

        //render fog off
        RenderSettings.fog = false;
        MapCamera.Render();
        RenderSettings.fog = true;
        //PlayerCamera.Render();

    }

    public void DisableThisObject()
    {
        Debug.Log("OFF MAP");
        onMap = false;
        //idisable overhead

        PlayerCompass.SetActive(false);

        foreach (GameObject c in checkpoints)
            c.GetComponentInChildren<SpriteRenderer>().enabled = false;

        RenderSettings.fog = true;

        i = 0;
        MapCamera.transform.position = player.transform.position + initialMapPos;
        tempSize = T1ZoomSize;
        MoveSpeed = T1MoveSpeed;
        MapCamera.orthographicSize = tempSize;
    }

}
