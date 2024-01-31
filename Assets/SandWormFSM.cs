using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormFSM : MonoBehaviour
{
    [Header("FSM Variables | Patrol")]
    [SerializeField] float distanceToAttack;
    
    [Header("FSM Variables | Attack")]
    [SerializeField] float warmUpTime;
    [SerializeField] float attackUpTime;
    [SerializeField] float timeToCloseMouth;
    [SerializeField] float burrowDistance;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float attackUpDistance;
    [SerializeField] float hangAirTime;
    [SerializeField] float attackDownTime;
    [SerializeField] float resumePatrolingTime;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator sandWormAnimator;


    public bool alive = true;

    SandWormState currentState = SandWormState.PATROLING;
    SandWormAttackState attackState = SandWormAttackState.NOT_ATTACKING;

    GameObject[] playerObjects;
    GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        playerObjects = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(FSM());
    }

    IEnumerator FSM()
    {
        while(alive)
        {
            switch(currentState)
            {
                case SandWormState.PATROLING:
                    Patrol();
                    break;
                case SandWormState.ATTACKING:
                    Attack();
                    break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    GameObject findTarget()
    {
        float leastDistance = -1;
        int playerIndex = -1;

        playerObjects = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < playerObjects.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, playerObjects[i].transform.position);
            if (leastDistance == -1 || dist <= leastDistance)
            {
                leastDistance = dist;
                playerIndex = i;
            }
        }

        return playerObjects[playerIndex];
    }

    void Patrol()
    {
        attackState = SandWormAttackState.NOT_ATTACKING;

        if (currentTarget == null)
        {
            currentTarget = findTarget();
        }

        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= distanceToAttack) {
            currentState = SandWormState.ATTACKING;
        }
    }

    void Attack()
    {
        if (attackState == SandWormAttackState.NOT_ATTACKING)
        {
            StartCoroutine(commenceAttack());
        }
    }

    IEnumerator commenceAttack()
    {
        attackState = SandWormAttackState.WARM_UP;

        Vector3 playerPos = currentTarget.transform.position;
        Vector3 groundPos = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(playerPos, Vector3.down, out hit, Mathf.Infinity, ~groundLayer))
        {
            groundPos = hit.point;
        }

        //set pos
        transform.position = groundPos + new Vector3(0f, burrowDistance, 0f);

        //Start WarmUp
        //Do particles

        yield return new WaitForSeconds(warmUpTime);

        //Start Attack
        attackState = SandWormAttackState.GOING_UP;
        sandWormAnimator.SetTrigger("StartAttack");

        Vector3 startingAttackPos = transform.position;
        Vector3 goalPos = startingAttackPos + new Vector3(0f, attackUpDistance, 0f);

        float totalTime = attackUpTime + timeToCloseMouth;

        float t = 0f;

        //lerp to position with animations
        while (t < totalTime)
        {
            transform.position = Vector3.Lerp(transform.position, Vector3.Lerp(startingAttackPos, goalPos, t / totalTime), Time.deltaTime * lerpSpeed);

            if (t >= attackUpTime && attackState == SandWormAttackState.GOING_UP)
            {
                attackState = SandWormAttackState.CLOSING_MOUTH;
                sandWormAnimator.SetTrigger("CloseMouth");
            }

            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        t = 0f;

        //wait in air
        yield return new WaitForSeconds(hangAirTime);

        //go down
        attackState = SandWormAttackState.GOING_DOWN;

        while (t < attackDownTime)
        {
            transform.position = Vector3.Lerp(goalPos, startingAttackPos, t / attackDownTime);

            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //wait after
        yield return new WaitForSeconds(resumePatrolingTime);

        //Done with attack
        currentTarget = findTarget();

        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= distanceToAttack) {
            currentState = SandWormState.ATTACKING;
        }
        else
        {
            currentState = SandWormState.PATROLING;
        }
    }
}

enum SandWormState
{
    PATROLING,
    ATTACKING
}

enum SandWormAttackState
{
    NOT_ATTACKING,
    WARM_UP,
    GOING_UP,
    CLOSING_MOUTH,
    GOING_DOWN
}
