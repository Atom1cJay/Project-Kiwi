using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NimAI : MonoBehaviour, IBehaviour
{
    [SerializeField] AIProfile profile;
    [SerializeField] GameObject SupriseIndicator;
    [SerializeField] AnimationHandler anim;
    [SerializeField] GameObject player;
    [SerializeField] float DistanceToSwitchWaypoints, PatrolRadius, TransitionTime, AggroSpeed, PatrolSpeed, PatrolPauseDelay;
    Vector3 initialPosition;
    Vector2 initialPos2D;
    AIState state, initialState, nextState;
    int currentHealth, damageToBeTaken;
    bool returning, canTakeDamage, canTransition, PatrolWait, PatrolPause;

    // Start is called before the first frame update
    void Awake()
    {
        //set up suprise
        SupriseIndicator.SetActive(false);

        //set up anim
        anim.currentMove("IDLE");

        //set initial state to patrol
        state = AIState.PATROL;

        //set initial pos
        initialPosition = transform.position;
        initialPos2D = new Vector2(initialPosition.x, initialPosition.z);

        //initialize misc vars
        returning = true;
        currentHealth = profile.InitialHealth;
        damageToBeTaken = 0;
        PatrolWait = false;
        PatrolPause = false;
        canTransition = true;
    }
    void Update()
    {

        Debug.DrawLine(transform.position, GetComponent<NavMeshAgent>().destination, Color.red);
    }

    public void Transition(NavMeshAgent agent)
    {
        if (canTransition)
        {
            canTransition = false;

            if (initialState == AIState.PATROL && nextState == AIState.CHASE)
            {

                SupriseIndicator.SetActive(true);
                anim.PauseAnimation();
            }
            if (nextState == AIState.PATROL && initialState == AIState.CHASE)
            {
                anim.PauseAnimation();

            }

            agent.isStopped = true;
            Invoke("TransitionTo", TransitionTime);
        }

    }

    //transitions the current state to the defined nextstate
    void TransitionTo()
    {

        state = nextState;
        canTransition = true;
        if (initialState == AIState.PATROL && nextState == AIState.CHASE)
        {
            SupriseIndicator.SetActive(false);
            anim.ResumeAnimation();

        }
        if (nextState == AIState.PATROL && initialState == AIState.CHASE)
        {
            anim.ResumeAnimation();

        }
    }

    public void Patrol(NavMeshAgent agent)
    {
        //set up animations
        anim.currentMove("IDLE");

        //dont stop
        agent.isStopped = false;

        //set up speed
        agent.speed = PatrolSpeed;

        //set up vector2s
        Vector2 pos = new Vector2(transform.position.x, transform.position.z);
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 currentDest = new Vector2(agent.destination.x, agent.destination.z);

        //Check to see if should transition first
        if (Vector2.Distance(pos, playerPos) <= profile.AggroDistance)
        {
            //Debug.Log("chase");
            initialState = AIState.PATROL;
            nextState = AIState.CHASE;
            state = AIState.TRANSITION;
        }

        //check if you should be returning
        if (Vector2.Distance(pos, initialPos2D) > PatrolRadius)
        {
            //Debug.Log("Returning" + Vector2.Distance(pos, initialPos2D));
            agent.SetDestination(initialPosition);
            returning = true;
        }
        else
        {
            //if waiting
            if (PatrolPause)
            {
                //stop
                agent.isStopped = true;
                if (!PatrolWait)
                {
                    PatrolWait = true;
                    Invoke("StopPatrolWaiting", 2f);
                }

            }
            else {
                //Check if waypoints should switch
                if (Vector2.Distance(pos, currentDest) < DistanceToSwitchWaypoints || returning)
                {
                    //Debug.Log("switching waypoints");
                    returning = false;
                    bool foundWaypoint = false;
                    Vector3 result = Vector3.zero;

                    //try 25 times?
                    for (int i = 0; i < 25; i++)
                    {
                        if (!foundWaypoint)
                        {
                            Vector2 randomPoint = initialPos2D + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * PatrolRadius;
                            Vector3 rp = new Vector3(randomPoint.x, initialPosition.y, randomPoint.y);


                            NavMeshHit hit;
                            if (NavMesh.SamplePosition(rp, out hit, 1f, 1))
                            {
                                result = hit.position;
                                foundWaypoint = true;
                            }
                        }
                    }

                    agent.SetDestination(result);
                    PatrolPause = true;
                }

            }
        }

    }

    void StopPatrolWaiting()
    {
        PatrolPause = false;
        PatrolWait = false;
    }
    public void DamageTaken(NavMeshAgent agent)
    {
        if (canTakeDamage)
        {
            Invoke("ReturnToOtherState", profile.ITime);
            canTakeDamage = false;
            currentHealth -= damageToBeTaken; 
        }

    }
    public void Chase(NavMeshAgent agent)
    {
        //set up animations
        anim.currentMove("RUNNING");

        //dont stop
        agent.isStopped = false;

        //set up speed
        agent.speed = AggroSpeed;

        //set up vector2s
        Vector2 pos = new Vector2(transform.position.x, transform.position.z);
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);

        //Debug.Log("aggro" + Vector2.Distance(pos, playerPos));

        //Check to see if should transition first
        if (Vector2.Distance(pos, playerPos) > profile.AggroDistance)
        {

            //Debug.Log("no longer aggro");
            nextState = AIState.PATROL;
            initialState = AIState.CHASE;
            state = AIState.TRANSITION;
        }

        //if not, reset destination to players position for live updates
        agent.SetDestination(player.transform.position);

    }

    void TakeDamage(int dmg)
    {
        if (state != AIState.DAMAGETAKEN)
        {
            canTakeDamage = true;
            initialState = state;
            damageToBeTaken = dmg;
            state = AIState.DAMAGETAKEN;
        }
    }

    void ReturnToOtherState()
    {
        state = initialState;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public AIState State()
    {
        return state;
    }
}
