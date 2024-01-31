using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormFSM : AMovingPlatform
{
    [Header("FSM Variables | Patrol")]
    [SerializeField] float distanceToAttack;
    
    [Header("FSM Variables | Attack")]
    [SerializeField] float warmUpTime;
    [SerializeField] float attackUpTime;
    [SerializeField] float timeToCloseMouth;
    [SerializeField] float timeAfterClosedMouth;
    [SerializeField] float burrowDistance;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float attackUpDistance;
    [SerializeField] float resumePatrolingTime;
    [SerializeField] Animator sandWormAnimator;

    [Header("SandWorm Vars")]
    [SerializeField] GameObject killBox;
    [SerializeField] float gravity;


    public bool alive = true;

    SandWormState currentState = SandWormState.PATROLING;
    SandWormAttackState attackState = SandWormAttackState.NOT_ATTACKING;

    GameObject[] playerObjects;
    GameObject currentTarget;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        playerObjects = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(FSM());
        killBox.SetActive(false);
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
        killBox.SetActive(false);

        Vector3 playerPos = currentTarget.transform.position;
        Vector3 groundPos = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(playerPos, Vector3.down, out hit, Mathf.Infinity, ~groundLayer))
        {
            groundPos = hit.point;
        }

        // Set position
        transform.position = groundPos + Vector3.up * burrowDistance;
        Vector3 startingPos = transform.position;

        // Start WarmUp
        // Do particles

        yield return new WaitForSeconds(warmUpTime);

        // Start Attack
        attackState = SandWormAttackState.GOING_UP;
        sandWormAnimator.SetTrigger("StartAttack");

        float totalTimeUp = attackUpTime + timeToCloseMouth + timeAfterClosedMouth;
        float initialVelocity = attackUpDistance / totalTimeUp - 0.5f * gravity * totalTimeUp;

        // Apply the calculated initial velocity
        velocity = Vector3.up * initialVelocity;

        yield return new WaitForSeconds(attackUpTime);

        attackState = SandWormAttackState.CLOSING_MOUTH;
        sandWormAnimator.SetTrigger("CloseMouth");

        yield return new WaitForSeconds(timeToCloseMouth);

        killBox.SetActive(true);

        yield return new WaitForSeconds(timeAfterClosedMouth);

        // Go down
        attackState = SandWormAttackState.GOING_DOWN;
        killBox.SetActive(false);

        while (transform.position.y >= startingPos.y)
        {
            yield return new WaitForEndOfFrame();
        }

        velocity = Vector3.zero;
        attackState = SandWormAttackState.WAITING;

        // Wait after
        yield return new WaitForSeconds(resumePatrolingTime);
        attackState = SandWormAttackState.NOT_ATTACKING;

        // Done with attack
        currentTarget = findTarget();

        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= distanceToAttack)
        {
            currentState = SandWormState.ATTACKING;
        }
        else
        {
            currentState = SandWormState.PATROLING;
        }

    }

    public override void Translate()
    {
        if (currentState == SandWormState.ATTACKING && attackState != SandWormAttackState.WARM_UP && attackState != SandWormAttackState.WARM_UP && attackState != SandWormAttackState.WAITING)
        {
            velocity += new Vector3(0f, gravity * Time.deltaTime, 0f);
            transform.position += velocity * Time.deltaTime;
        }
    }

    public override void Rotate()
    {
        ///do nothing
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
    MOUTH_CLOSED,
    GOING_DOWN,
    WAITING
}
