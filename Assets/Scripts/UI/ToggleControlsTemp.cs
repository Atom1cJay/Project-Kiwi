using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ToggleControlsTemp : MonoBehaviour
{
    bool mode = false;
    string origText;
    Text txt;

    private void Awake()
    {
        txt = GetComponent<Text>();
        origText = txt.text;
        txt.text = "C for Controls";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            mode = !mode;
            txt.text = mode ? origText : "C for Controls";
        }
    }
}
