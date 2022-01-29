using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UISceneController : MonoBehaviour, UIInterface
{
    [SerializeField] string scene;
    [SerializeField] Canvas canvas;
    [SerializeField] ParticleSystem ps;


    public void DisableThisObject()
    {

    }

    public void EnableThisObject()
    {
        StartCoroutine(NextSceneAnimation());
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    IEnumerator NextSceneAnimation()
    {
        //we're disabling any further user input!

        EventSystem.current.enabled = false;

        //we're setting this renderMode to no longer be an overlay and instead a canvas for our rotating camera.

        canvas.renderMode = RenderMode.ScreenSpaceCamera;


        //as the canvas with WanderLeaf is now part of the rotating camera overlay, we can play our particle overlay over that
        yield return new WaitForSeconds(.01f);

        //play our particles
        ps.Play();

        yield return new WaitForSeconds(5f);
        //next scene
        SceneManager.LoadScene(scene);

    }
}
