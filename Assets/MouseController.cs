using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public static MouseController instance;
    bool click, released, releaseClick;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        click = false;
        released = true;
        releaseClick = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (releaseClick)
        {
            click = false;
            releaseClick = false;
        }

        if (Input.GetMouseButton(0) && released)
        {
            click = true;
            released = false;
            releaseClick = true;

        }


        if (!Input.GetMouseButton(0))
        {
            released = true;
        }
    }

    public bool OnClick()
    {
        return click;
    }
}
