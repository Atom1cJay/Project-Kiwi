using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentState : MonoBehaviour, TransitionCheck
{
    [SerializeField] string currentMove;
    string lastMove = "";
    string move = "";

    public bool StateCheck()
    {
        return currentMove.Equals(move);
    }

    public void UpdateMoves(string lastMove, string move)
    {
        this.lastMove = lastMove;
        this.move = move;
    }


}
