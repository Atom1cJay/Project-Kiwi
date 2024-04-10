using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCloudFSM : MonoBehaviour
{
    [Header("Moving Vars")]
    [SerializeField] float moveSpeed;
    [SerializeField] float moveGoalVisualHeight;
    [SerializeField] List<float> movingTornadoGoalDissolves;

    [Header("Attacking Vars")]
    [SerializeField] float damage;
    [SerializeField] float attackGoalVisualHeight;
    [SerializeField] List<float> attackTornadoGoalDissolves;

    [Header("Vunerable Vars")]
    [SerializeField] float vunerableGoalVisualHeight;
    [SerializeField] List<float> vunerableTornadoGoalDissolves;

    [Header("Mesh /Visual References")]
    [SerializeField] List<MeshRenderer> tornadoParts;
    [SerializeField] GameObject visualParent;

    bool alive;
    KingCloudState currentState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator FSM()
    {
        while (alive)
        {
            //Debug.Log("fml");
            switch (currentState)
            {
                case KingCloudState.VUNERABLE:
                    Vunerable();
                    break;
                case KingCloudState.ATTACKING:
                    Attack();
                    break;
                case KingCloudState.MOVING:
                    Move();
                    break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void Move()
    {

    }

    private void Attack()
    {
        throw new NotImplementedException();
    }

    private void Vunerable()
    {
        throw new NotImplementedException();
    }
}


public enum KingCloudState
{
    VUNERABLE,
    ATTACKING,
    MOVING
}
