using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToThisMove : MonoBehaviour, TransitionCheck
{
    [SerializeField] string currentMove;

    string lastMove = "";
    string move = "";

    public bool StateCheck()
    {
        return move.Equals(currentMove) && !lastMove.Equals(move);
    }

    public void UpdateMoves(string lastMove, string move)
    {
        this.lastMove = lastMove;
        this.move = move;
    }
}
