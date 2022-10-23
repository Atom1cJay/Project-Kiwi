using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransitionOccurrence : FinishableOccurrence
{
    [Header("Leave Blank = Any Is Okay")]
    [SerializeField] string preTransitionMove;
    [SerializeField] string postTransitionMove;
    bool active = false;

    void Start()
    {
        MoveExecuter.instance.OnMoveChanged += (oldMove, newMove) =>
        {
            if (!active)
            {
                ConsiderStarting(oldMove, newMove);
            }
            else
            {
                InvokeOnOccurrenceFinish();
                active = false;
            }
        };
    }

    void ConsiderStarting(IMoveImmutable oldMove, IMoveImmutable newMove)
    {
        bool preCond = preTransitionMove == "" || (oldMove != null && preTransitionMove == oldMove.AsString());
        bool postCond = postTransitionMove == "" || postTransitionMove == newMove.AsString();
        if (preCond && postCond)
        {
            InvokeOnOccurrenceStart();
            active = true;
        }
    }
}
