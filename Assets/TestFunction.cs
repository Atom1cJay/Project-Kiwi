using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestFunction : MonoBehaviour
{
    [SerializeField] UnityEvent testEvent;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            testEvent.Invoke();

    }
}