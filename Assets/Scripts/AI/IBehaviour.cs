using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IBehaviour
{
    //What state is the AI in
    AIState State();
    //is the AI alive

    bool IsAlive();
    
    //Patrol action
    void Patrol(NavMeshAgent agent);


    //Chase action
    void Chase(NavMeshAgent agent);


    //Transition action
    void Transition(NavMeshAgent agent);

    //DamageTaken action
    void DamageTaken(NavMeshAgent agent);
}
