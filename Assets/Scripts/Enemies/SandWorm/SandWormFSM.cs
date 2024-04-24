using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SandWormFSM : AMovingPlatform
{
    [Header("FSM Variables | Patrol")]
    [SerializeField] float distanceToAttack;
    [SerializeField] float radiusPatrol;
    [SerializeField] float distanceToNextPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float patrolLerpSpeed;
    [SerializeField] ParticleSystem patrolParticleSystem;
    [SerializeField] float bodySize;
    [SerializeField] Sound patrolSound;

    [Header("FSM Variables | Attack")]
    [SerializeField] float warmUpTime;
    [SerializeField] float hangTime;
    [SerializeField] float pctUpTimeToCloseMouth;
    [SerializeField] float timeToCloseMouth;
    [SerializeField] float timeBeforeDisablingKillBox;
    [SerializeField] float pctUpTimeParticles;
    [SerializeField] float hangTimeLerpSpeed;
    [SerializeField] float gravity;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float attackUpDistance;
    [SerializeField] float resumePatrolingTime;
    [SerializeField] Animator sandWormAnimator;
    [SerializeField] ParticleSystem preAttackParticleSystem;
    [SerializeField] ParticleSystem attackParticleSystem;
    [SerializeField] UnityEvent particlesBeforeAttack;
    [SerializeField] Sound attackSound;

    [Header("SandWorm Vars")]
    [SerializeField] GameObject killBox;
    [SerializeField] Color baseColor, angryColor;
    [SerializeField] SkinnedMeshRenderer meshRenderer;

    public bool alive = true;

    //patrol
    Vector3 currentCheckpoint;
    Vector3 initialPos;
    Vector3 lastPos;
    Vector3 patrolVelocity = Vector3.zero;

    //state
    [Header("DONT MODIFY FOR TESTING")]
    public SandWormState currentState = SandWormState.PATROLING;
    public SandWormAttackState attackState = SandWormAttackState.NOT_ATTACKING;

    //player and target
    GameObject currentTarget;

    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        //disable colliders for raycast
        RaycastHit hit;
        meshRenderer.materials[0].SetColor("Albedo", baseColor);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundLayer))
        {
            Vector3 goalShadowPos = hit.point;
            transform.position = goalShadowPos - (GameObject.FindGameObjectWithTag("Sun").transform.transform.forward * 0.01f);
        }

        initialPos = transform.position;
        lastPos = transform.position;
        currentCheckpoint = checkpointPos();

        patrolParticleSystem.Play();
        StartCoroutine(FSM());

        killBox.SetActive(false);
    }

    IEnumerator FSM()
    {
        while(alive)
        {
            //Debug.Log("fml");
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

        List<GameObject> possibleTargets = new List<GameObject>();

        possibleTargets.Add(GameObject.FindGameObjectWithTag("Player"));

        SandWormBait[] bait = FindObjectsOfType<SandWormBait>();

        foreach (SandWormBait swb in bait)
            possibleTargets.Add(swb.gameObject);

        for (int i = 0; i < possibleTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, possibleTargets[i].transform.position);
            if (dist <= distanceToAttack && leastDistance == -1 || dist <= leastDistance)
            {
                leastDistance = dist;
                playerIndex = i;
            }
        }


        if (playerIndex == -1)
            return null;

        Debug.Log("found target : + " + possibleTargets[playerIndex].name);

        return possibleTargets[playerIndex];
    }

    Vector3 checkpointPos()
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        // Calculate the X and Z coordinates using polar coordinates
        float xPos = initialPos.x + radiusPatrol * Mathf.Cos(randomAngle);
        float zPos = initialPos.z + radiusPatrol * Mathf.Sin(randomAngle);

        // Create the position vector
        return new Vector3(xPos, initialPos.y, zPos);
    }

    void Patrol()
    {
        attackState = SandWormAttackState.NOT_ATTACKING;

        if (currentTarget == null)
        {
            currentTarget = findTarget();
        }


        if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) <= distanceToAttack && canJumpWithoutHittingAnything(currentTarget.transform.position)) {
            currentState = SandWormState.ATTACKING;
            patrolParticleSystem.Stop();
        }
        else if (Vector3.Distance(transform.position, currentCheckpoint) <= distanceToNextPoint)
        {
            currentCheckpoint = checkpointPos();
        }
        else
        {
            //Follow Path Here
            patrolVelocity = Vector3.Lerp(patrolVelocity, (currentCheckpoint - transform.position).normalized * moveSpeed, Time.deltaTime * patrolLerpSpeed);
            transform.position += patrolVelocity * Time.deltaTime;
            lastPos = transform.position;
        }
    }

    void Attack()
    {
        if (attackState == SandWormAttackState.NOT_ATTACKING)
        {
            StartCoroutine(commenceAttack());
        }
    }

    void closeMouth()
    {

        //kill if in mouth
        killBox.SetActive(true);

        Invoke("stopKillBox", timeBeforeDisablingKillBox);
    }

    void stopKillBox()
    {
        //disable killbox
        killBox.SetActive(false);
    }

    IEnumerator commenceAttack()
    {
        attackState = SandWormAttackState.WARM_UP;
        killBox.SetActive(false);

        Vector3 groundPos = Vector3.zero;

        StickToGround shadow = currentTarget.GetComponentInChildren<StickToGround>();
        
        if (shadow != null)
        {
            groundPos = shadow.gameObject.transform.position;
        }
        else
        {
            groundPos = currentTarget.transform.position;
        }

        // Set position
        transform.position = groundPos + Vector3.up;

        preAttackParticleSystem.gameObject.transform.position = groundPos;

        preAttackParticleSystem.Play();
        Vector3 startingPos = transform.position;
        Debug.Log("starting pos " + startingPos);

        // Start WarmUp
        // Do particles

        patrolSound.Play(transform);
        yield return new WaitForSeconds(warmUpTime);

        preAttackParticleSystem.Stop();
        // Start Attack
        attackState = SandWormAttackState.GOING_UP;
        sandWormAnimator.SetTrigger("StartAttack");

        patrolSound.Stop();
        attackParticleSystem.Play();
        attackSound.Play(transform);


        //gravity = (attackUpDistance / (totalTime * totalTime)) - initialVelocity / totalTime;
        float initialVelocity = Mathf.Sqrt(2 * -gravity * attackUpDistance);


        float totalTimeUp = initialVelocity / -gravity;//((-initialVelocity - Mathf.Sqrt(initialVelocity * initialVelocity - 4 * gravity * attackUpDistance)) / 2 * gravity) / 2;

        //gravity = attackUpDistance / totalTime + 0.5f * gravity * totalTime;
        Debug.Log("velocity : " + initialVelocity + "d : " + attackUpDistance + ", t" + totalTimeUp + ", timeto close " + timeToCloseMouth);
        //float initialVelocity = (attackUpDistance - 0.5f * gravity * ((t1 * t1) - 2 * t1 * totalTimeUp) + 0.5f * hangTimeGravity * Mathf.Pow(totalTimeUp - t1, 2)) / totalTimeUp;
        // Apply the calculated initial velocity
        velocity = Vector3.up * initialVelocity;

        meshRenderer.materials[0].SetColor("Albedo", baseColor);

        bool particlesReleased = false;
        bool openMouth = false;


        //while going up ramp up color
        float startTime = Time.time;
        while (Time.time < startTime + totalTimeUp)
        {
            yield return null;
            attackParticleSystem.gameObject.transform.position = groundPos;
            float pct = (Time.time - startTime) / totalTimeUp;

            if (!particlesReleased && pct >= pctUpTimeParticles)
            {
                particlesReleased = true;
                particlesBeforeAttack.Invoke();
            }

            if (!openMouth && pct >= pctUpTimeToCloseMouth)
            {
                openMouth = true;
                sandWormAnimator.SetTrigger("CloseMouth");
                Invoke("closeMouth", timeToCloseMouth);

            }

            meshRenderer.materials[0].SetColor("Albedo", Color.Lerp(baseColor, angryColor, pct));

        }

        meshRenderer.materials[0].SetColor("Albedo", angryColor);

        //enter hang time gravity
        attackState = SandWormAttackState.HANGING;

        yield return new WaitForSeconds(hangTime);

        // Go down
        attackState = SandWormAttackState.GOING_DOWN;

        yield return new WaitForSeconds(timeBeforeDisablingKillBox);
        killBox.SetActive(false);

        //stop when y is lowe enough
        while (transform.position.y >= startingPos.y)
        {
            attackParticleSystem.gameObject.transform.position = groundPos;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        attackParticleSystem.Stop();

        velocity = Vector3.zero;
        attackState = SandWormAttackState.WAITING;
        sandWormAnimator.SetTrigger("Reset");

        // Wait after
        yield return new WaitForSeconds(resumePatrolingTime);
        attackState = SandWormAttackState.NOT_ATTACKING;

        transform.position = lastPos;

        // Done with attack
        meshRenderer.materials[0].color = baseColor;
        currentTarget = findTarget();

        if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) <= distanceToAttack)
        {
            currentState = SandWormState.ATTACKING;
        }
        else
        {
            currentState = SandWormState.PATROLING;
            patrolParticleSystem.Play();
            patrolVelocity = Vector3.zero;
        }

    }

    int POINTS_CHECKED_AROUND_PLAYER = 8;

    bool canJumpWithoutHittingAnything(Vector3 pos)
    {
        bool hitAnything = false;

        //Debug.DrawRay(pos, Vector3.up * attackUpDistance);
        if (Physics.Raycast(pos, Vector3.up, attackUpDistance, groundLayer))
        {
            hitAnything = true;
        }

        // Cast rays at points around the player in a circle
        for (int i = 0; i < POINTS_CHECKED_AROUND_PLAYER; i++) 
        {
            // Calculate the angle for this point around the circle
            float angle = i * (360f / POINTS_CHECKED_AROUND_PLAYER);

            // Calculate the direction vector for this point
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            // Calculate the position of the point around the circle
            Vector3 circlePoint = pos + direction * bodySize / 2;

            //Debug.DrawRay(circlePoint, Vector3.up * attackUpDistance);
            if (Physics.Raycast(circlePoint, Vector3.up, attackUpDistance, groundLayer))
            {
                hitAnything = true;
            }
        }

        return !hitAnything;
    }



    public override void Translate()
    {
        if (currentState == SandWormState.ATTACKING && attackState != SandWormAttackState.WARM_UP && attackState != SandWormAttackState.WAITING && attackState != SandWormAttackState.HANGING)
        {
            velocity += new Vector3(0f, gravity * Time.deltaTime, 0f);
            transform.Translate(velocity * Time.deltaTime);
        }
        else if (attackState == SandWormAttackState.HANGING)
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * hangTimeLerpSpeed);
            transform.Translate(velocity * Time.deltaTime);
        }
    }

    public override void Rotate()
    {
        ///do nothing
    }
}

public enum SandWormState
{
    PATROLING,
    ATTACKING
}

public enum SandWormAttackState
{
    NOT_ATTACKING,
    WARM_UP,
    GOING_UP,
    HANGING,
    GOING_DOWN,
    WAITING
}
