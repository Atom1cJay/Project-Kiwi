using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystemV2 : MonoBehaviour
{
    TransitionCheck[] controllers;
    [SerializeField] MoveExecuter me;
    string lastMove = "";

    void Awake()
    {
        controllers = GetComponentsInChildren<TransitionCheck>();
    }

    void Update()
    {
        string currentMove = MoveExecuter.GetCurrentMove().AsString();
        foreach (TransitionCheck c in controllers)
        {
            c.UpdateMoves(lastMove, currentMove);
        }
        lastMove = currentMove;
    }
}
