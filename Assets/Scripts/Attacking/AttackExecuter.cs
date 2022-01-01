using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveExecuter))]
public class AttackExecuter : MonoBehaviour
{
    [SerializeField] Transform hitboxParent;
    // The parent for the hitboxes to be spawned under
    MoveExecuter me;
    Attack[] curAttacks;

    private void Awake()
    {
        me = GetComponent<MoveExecuter>();
        me.OnMoveChanged.AddListener(() => ConsiderAttackExecution());
    }

    void ConsiderAttackExecution()
    {
        if (curAttacks != null)
        {
            // Since the move has changed, the attacks for the last move should end.
            foreach (Attack a in curAttacks)
            {
                a.DisableAttack();
            }
        }

        curAttacks = me.GetCurrentMove().GetAttack();

        if (curAttacks != null)
        {
            foreach (Attack a in curAttacks)
            {
                a.EnableAttack(hitboxParent);
            }
        }
    }
}
