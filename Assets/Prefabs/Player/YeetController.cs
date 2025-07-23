using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetController : MonoBehaviour
{
    [HideInInspector]
    public static YeetController instance;
    public float initialYVelocity;
    public bool moveXHorizontal;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

}
