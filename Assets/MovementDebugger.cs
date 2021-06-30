using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveExecuter))]
public class MovementDebugger : MonoBehaviour
{
    MoveExecuter me;
    string storedMoveString = "nothing";

    private void Start()
    {
        me = GetComponent<MoveExecuter>();
    }

    void Update()
    {
        string curMoveString = me.GetCurrentMove().AsString();
        if (curMoveString != storedMoveString)
        {
            print("SWITCH: " + storedMoveString + " -> " + curMoveString);
            storedMoveString = curMoveString;
        }
    }
}
