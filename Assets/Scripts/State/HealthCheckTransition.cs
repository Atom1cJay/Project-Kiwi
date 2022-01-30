using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCheckTransition : MonoBehaviour, TransitionCheck
{
    [SerializeField] string currentMove;
    [SerializeField] bool onNegHealthChange;

    int currentHealth = 0;

    void Start()
    {
        currentHealth = PlayerHealth.instance.hp;
    }

    string lastMove = "";
    string move = "";

    public bool StateCheck()
    {
        int temp = PlayerHealth.instance.hp;

        if (move.Equals(currentMove) && !lastMove.Equals(move))
        {
            if (temp == currentHealth - 1)
            {
                currentHealth--;
                return onNegHealthChange;
            }
            else
            {
                return !onNegHealthChange;
            }

        }

        return false;
    }

    public void UpdateMoves(string lastMove, string move)
    {
        this.lastMove = lastMove;
        this.move = move;
    }
}
