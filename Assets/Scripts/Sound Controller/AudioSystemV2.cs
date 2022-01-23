using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystemV2 : MonoBehaviour
{
    TransitionCheck[] controllers;
    [SerializeField] MoveExecuter me;
    string lastMove = "";
    // Start is called before the first frame update
    void Awake()
    {
        controllers = GetComponentsInChildren<TransitionCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        string currentMove = me.GetCurrentMove().AsString();
        Debug.Log(currentMove);
        foreach (TransitionCheck c in controllers)
        {
            c.UpdateMoves(lastMove, currentMove);
        }
        lastMove = currentMove;
    }
}
