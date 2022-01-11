using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCheckTransition : MonoBehaviour, TransitionCheck
{
    [SerializeField] string moveToCheckSpeed, moveToStart;
    [SerializeField] float speedToHit;
    [SerializeField] MoveExecuter me;

    bool hitSpeed = false;

    string lastMove = "";
    string move = "";
    public bool StateCheck()
    {
        float speed = me.GetCurrentMove().GetHorizSpeedThisFrame().magnitude;
        bool value = false;

        if (moveToCheckSpeed.Equals(move))
        {
            if (speed >= speedToHit)
            {
                hitSpeed = true;
            }
        }

        if (!(moveToStart.Equals(move) || moveToCheckSpeed.Equals(move)))
        {
            hitSpeed = false;
        }

        if (moveToStart.Equals(move) && moveToCheckSpeed.Equals(lastMove))
        {
            if (hitSpeed)
            {
                value = true;
            }
            hitSpeed = false;
        }

        return value;
    }

    public void UpdateMoves(string lastMove, string move)
    {
        this.lastMove = lastMove;
        this.move = move;
    }
}
