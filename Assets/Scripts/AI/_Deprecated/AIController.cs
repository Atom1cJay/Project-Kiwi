using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(IBehaviour))]
public class AIController : MonoBehaviour
{

    private NavMeshAgent agent;

    IBehaviour AI;

    // Start is called before the first frame update
    void Awake()
    {
        AI = GetComponent<IBehaviour>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(FSM());
    }

    IEnumerator FSM()
    {
        while (AI.IsAlive())
        {
            switch (AI.State())
            {
                case AIState.PATROL:
                    AI.Patrol(agent);
                    break;
                case AIState.CHASE:
                    AI.Chase(agent);
                    break;
                case AIState.TRANSITION:
                    AI.Transition(agent);
                    break;
                case AIState.DAMAGETAKEN:
                    AI.DamageTaken(agent);
                    break;
            }
            yield return null;
        }
    }
}
