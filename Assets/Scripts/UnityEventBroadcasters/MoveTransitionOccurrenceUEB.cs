using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// UnityEventBroadcaster that broadcasts an event whenever the player's move is
/// changed to a specific one.
/// </summary>
public class MoveTransitionOccurrenceUEB : MonoBehaviour
{
    [Header("Leave Blank = Any Is Okay")]
    [SerializeField] string preTransitionMove;
    [SerializeField] string postTransitionMove;
    public UnityEvent onPostTransitionMove; // Called when we go from pre to post transition move
    public UnityEvent onPostTransitionMoveEnd;
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
                onPostTransitionMoveEnd.Invoke();
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
            onPostTransitionMove.Invoke();
            active = true;
        }
    }
}
