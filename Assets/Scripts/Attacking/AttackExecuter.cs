using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExecuter : MonoBehaviour
{
    [SerializeField] Transform hitboxParent;
    // The parent for the hitboxes to be spawned under
    Attack[] curAttacks;

    private void Start()
    {
        MoveExecuter.instance.OnMoveChanged += (oldMove, newMove) => ConsiderAttackExecution(newMove);
    }

    void ConsiderAttackExecution(IMoveImmutable newMove)
    {
        if (curAttacks != null)
        {
            // Since the move has changed, the attacks for the last move should end.
            foreach (Attack a in curAttacks)
            {
                a.DisableAttack();
            }
        }

        curAttacks = newMove.GetAttack();

        if (curAttacks != null)
        {
            foreach (Attack a in curAttacks)
            {
                a.EnableAttack(hitboxParent);
            }
        }
    }
}
