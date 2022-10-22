using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopChainDuringMove : MonoBehaviour
{
    [SerializeField] TransitionChain tc;

    private StateController StateChecker;


    private void Awake()
    {
        StateChecker = GetComponent<StateController>();
    }

    void Update()
    {
        if (StateChecker.StateCheck())
        {
            tc.StopChain();
        }
    }

}
