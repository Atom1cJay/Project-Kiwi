using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICameraOptions : MonoBehaviour
{
    [SerializeField] CameraControl cc;
    [SerializeField] bool X, Y;

    private void Start()
    {
        //set up camera
        Toggle();
    }

    public void Toggle()
    {
        if (X)
            cc.SetXInverted(GetComponent<Toggle>().isOn);
        else
            cc.SetYInverted(GetComponent<Toggle>().isOn);

    }
}

