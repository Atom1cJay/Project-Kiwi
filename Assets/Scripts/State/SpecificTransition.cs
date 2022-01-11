using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificTransition : MonoBehaviour, TransitionCheck
{
    [SerializeField] string currentMove, previousMove;
    [SerializeField] bool flipBool;

    string lastMove = "";
    string move = "";

    public bool StateCheck()
    {
        if (!flipBool)
            return move.Equals(currentMove) && lastMove.Equals(previousMove);
        else
            return (move.Equals(currentMove) && !lastMove.Equals(previousMove) && !lastMove.Equals(move));
    }

    public void UpdateMoves(string lastMove, string move)
    {
        this.lastMove = lastMove;
        this.move = move;
    }
}
