using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveExecuter))]
public class AttackExecuter : MonoBehaviour
{
    [SerializeField] Transform hitboxParent;
    // The parent for the hitboxes to be spawned under
    MoveExecuter me;
    Attack curAttack;

    private void Awake()
    {
        me = GetComponent<MoveExecuter>();
        me.OnMoveChanged.AddListener(() => ConsiderAttackExecution());
    }

    void ConsiderAttackExecution()
    {
        if (curAttack != null)
        {
            // Since the move has changed, the attack for the last move should end.
            curAttack.DisableAttack();
        }

        curAttack = me.GetCurrentMove().GetAttack();

        if (curAttack != null)
        {
            curAttack.EnableAttack(hitboxParent);
        }
    }
}
