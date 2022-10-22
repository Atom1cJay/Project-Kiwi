using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStopper : MonoBehaviour
{
    private StateController StateChecker;
    [SerializeField] string soundName;

    private void Awake()
    {
        StateChecker = GetComponent<StateController>();
    }


    // Update is called once per frame
    void Update()
    {
        if (StateChecker.StateCheck())
        {
            AudioMasterController.instance.StopSound(soundName);
        }
    }
}
