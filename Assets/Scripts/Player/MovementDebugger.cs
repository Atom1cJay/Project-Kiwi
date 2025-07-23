using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDebugger : MonoBehaviour
{
    string storedMoveString = "nothing";

    void Start()
    {
        MoveExecuter.instance.OnMoveChanged += (oldMove, newMove) => LogSwitch(newMove);
    }

    void LogSwitch(IMoveImmutable newMove)
    {
        string curMoveString = newMove.AsString();
        print("SWITCH: " + storedMoveString + " -> " + curMoveString);
        storedMoveString = curMoveString;
    }
}
