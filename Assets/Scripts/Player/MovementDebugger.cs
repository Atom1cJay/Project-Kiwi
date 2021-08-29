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
        me.OnMoveChanged.AddListener(() => LogSwitch());
    }

    void LogSwitch()
    {
        string curMoveString = me.GetCurrentMove().AsString();
        print("SWITCH: " + storedMoveString + " -> " + curMoveString);
        storedMoveString = curMoveString;
    }
}
