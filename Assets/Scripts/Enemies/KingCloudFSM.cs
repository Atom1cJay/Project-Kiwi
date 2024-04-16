using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCloudFSM : MonoBehaviour
{
    [Header("Moving Vars")]
    [SerializeField] float moveSpeed;
    [SerializeField] float radiusPatrol;
    [SerializeField] float distanceToAttack;
    [SerializeField] float distanceToNextPoint;

    [Header("Attacking Vars")]
    [SerializeField] float timeToChargeBack;
    [SerializeField] float chargeBackSpeed;
    [SerializeField] float playerProximity;
    [SerializeField] float overstepDuration;
    [SerializeField] float chargeSpeed;
    [SerializeField] float timeToReturnToMove;
    [SerializeField] float chanceToGoVunerable;
    [SerializeField] Damager ourDamager;
    [SerializeField] float bufferTime;

    [Header("Vunerable Vars")]
    [SerializeField] float timeToBeVunerable;
    [SerializeField] float vunerableSpeedMultiplier;

    [Header("Waiting Vars")]
    [SerializeField] float waitingTime;

    [Header("Colliders Vars")]
    [SerializeField] List<Collider> attackingColliders;
    [SerializeField] List<Collider> vunerableColliders;
    [SerializeField] List<Collider> waitingColliders;

    [Header("Color Vars")]
    [SerializeField] [ColorUsageAttribute(false, true)] List<Color> waitingTornadoGoalColors;
    [SerializeField] [ColorUsageAttribute(false, true)] List<Color> movingTornadoGoalColors;
    [SerializeField] [ColorUsageAttribute(false, true)] List<Color> attackTornadoGoalColors;
    [SerializeField] [ColorUsageAttribute(false, true)] List<Color> vunerableTornadoGoalColors;

    [Header("Dissolve vars")]
    [SerializeField] List<float> waitingTornadoGoalDissolves;
    [SerializeField] List<float> movingTornadoGoalDissolves;
    [SerializeField] List<float> attackTornadoGoalDissolves;
    [SerializeField] List<float> vunerableTornadoGoalDissolves;

    [Header ("Pos vars")]
    [SerializeField] List<Vector3> waitingTornadoGoalPositions;
    [SerializeField] List<Vector3> movingTornadoGoalPositions;
    [SerializeField] List<Vector3> attackTornadoGoalPositions;
    [SerializeField] List<Vector3> vunerableTornadoGoalPositions;

    [Header ("Scale vars")]
    [SerializeField] List<Vector3> waitingTornadoGoalScales;
    [SerializeField] List<Vector3> movingTornadoGoalScales;
    [SerializeField] List<Vector3> attackTornadoGoalScales;
    [SerializeField] List<Vector3> vunerableTornadoGoalScales;

    [Header("Particle Vars")]
    [SerializeField] List<float> waitingTornadoGoalParticleEmissions;
    [SerializeField] List<float> movingTornadoGoalParticleEmissions;
    [SerializeField] List<float> attackTornadoGoalParticleEmissions;
    [SerializeField] List<float> vunerableTornadoGoalParticleEmissions;

    [Header("Lerp Rotation Var")]
    [SerializeField] List<float> waitingTornadoGoalLerpInfo;
    [SerializeField] List<float> movingTornadoGoalLerpInfo;
    [SerializeField] List<float> attackTornadoGoalLerpInfo;
    [SerializeField] List<float> vunerableTornadoGoalLerpInfo;

    [Header("Mesh /Visual References")]
    [SerializeField] List<MeshRenderer> tornadoParts;
    [SerializeField] List<ParticleSystem> particleParts;
    [SerializeField] LerpRotation rotationLerper;
    [SerializeField] GameObject visualParent;
    [SerializeField] float moveLerpSpeed;
    [SerializeField] float visualLerpSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] GameObject cloudObject;

    [Header("Health Info")]
    [SerializeField] int health;
    [SerializeField] GameObject canvasParent;
    [SerializeField] GameObject healthBarObject;
    [SerializeField] List<GameObject> stageObjects;


    public KingCloudState currentState;

    Vector3 currentCheckpoint;
    public Vector3 velocity;
    public Vector3 goalVelocity;

    bool alive;
    bool isAttacking;
    bool isVunerable;
    bool isWaiting;
    GameObject player;
    Vector3 ir;


    // Start is called before the first frame update
    void Start()
    {
        //set up data
        ir = transform.eulerAngles;
        isAttacking = false;
        isVunerable = false;
        isWaiting = false;
        player = GameObject.FindGameObjectWithTag("Player");
        currentCheckpoint = checkpointPos();
        alive = true;

        //health logic

        for (int i = 0; i < health; i ++)
        {
            Instantiate(healthBarObject, canvasParent.transform);
        }

        foreach (GameObject level in stageObjects)
        {
            level.SetActive(false);
        }

        stageObjects[health - 1].SetActive(true);

        //Start up

        startVisuals();
        StartCoroutine(FSM());
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
                case KingCloudState.WAITING:
                    Wait();
                    break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    void Update()
    {
        if (currentState == KingCloudState.MOVING || currentState == KingCloudState.ATTACKING || currentState == KingCloudState.WAITING)
        {
            velocity = Vector3.Lerp(velocity, goalVelocity, Time.deltaTime * moveLerpSpeed);
            velocity = new Vector3(velocity.x, 0f, velocity.z);
            transform.position += velocity * Time.deltaTime;
            //rotate visuals
            Quaternion q = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity), rotateSpeed);
            Vector3 v = q.eulerAngles;

            transform.eulerAngles = new Vector3(ir.x, v.y, ir.z);
        }
    }

    #region damage

    public void takeDamage()
    {
        health -= 1;
        Destroy(canvasParent.transform.GetChild(0).gameObject);
        foreach (GameObject level in stageObjects)
        {
            level.SetActive(false);
        }

        stageObjects[health - 1].SetActive(true);

        //do cutscene

        CancelInvoke("stopVunerable");
        isVunerable = false;
        currentState = KingCloudState.WAITING;
    }

    public void bufferDamage()
    {
        ourDamager.enabled = false;
        Invoke("enableDamage", bufferTime);
    }

    void enableDamage()
    {
        ourDamager.enabled = true;
    }

    #endregion

    #region visuals


    void lerpVisuals()
    {
        for (int i = 0; i < tornadoParts.Count; i++)
        {
            //mesh stuff
            MeshRenderer mr = tornadoParts[i];
            float currentDisolve = mr.material.GetFloat("_Dissolve");
            mr.material.SetFloat("_Dissolve", Mathf.Lerp(currentDisolve, getTargetDissolve(i, currentState), Time.deltaTime * visualLerpSpeed));
            Color currentColor = mr.material.GetColor("_Color");
            mr.material.SetColor("_Color", Color.Lerp(currentColor, getTargetColor(i, currentState), Time.deltaTime * visualLerpSpeed));

            //positioning stuff
            GameObject meshObject = mr.gameObject;
            meshObject.transform.localPosition = Vector3.Lerp(meshObject.transform.localPosition, getTargetPosition(i, currentState), Time.deltaTime * visualLerpSpeed);
            meshObject.transform.localScale = Vector3.Lerp(meshObject.transform.localScale, getTargetScale(i, currentState), Time.deltaTime * visualLerpSpeed);
        }

        for (int i = 0; i < particleParts.Count; i++)
        {

            ParticleSystem particleSystem = particleParts[i];

            //particles

            ParticleSystem.EmissionModule emission = particleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(emission.rateOverTime.constant, getTargetEmission(i, currentState), Time.deltaTime * visualLerpSpeed);
        }
    }

    void startVisuals()
    {
        for (int i = 0; i < tornadoParts.Count; i++)
        {
            //mesh stuff
            MeshRenderer mr = tornadoParts[i];
            float currentDisolve = mr.material.GetFloat("_Dissolve");
            mr.material.SetFloat("_Dissolve", getTargetDissolve(i, currentState));
            Color currentColor = mr.material.GetColor("_Color");
            mr.material.SetColor("_Color", getTargetColor(i, currentState));

            //positioning stuff
            GameObject meshObject = mr.gameObject;
            meshObject.transform.localPosition = getTargetPosition(i, currentState);
            meshObject.transform.localScale = getTargetScale(i, currentState);
        }

        for (int i = 0; i < particleParts.Count; i++)
        {

            ParticleSystem particleSystem = particleParts[i];

            //particles

            ParticleSystem.EmissionModule emission = particleSystem.emission;
            emission.rateOverTime = getTargetEmission(i, currentState);
        }

        setLerpInfo(currentState);
    }

    #endregion

    #region moving
    private void Move()
    {
        lerpVisuals();
        enableColliders(currentState);

        if (Vector3.Distance(transform.position, player.transform.position) <= distanceToAttack)
        {
            currentState = KingCloudState.ATTACKING;
        }
        else if (Vector3.Distance(transform.position, currentCheckpoint) <= distanceToNextPoint)
        {
            currentCheckpoint = checkpointPos();
        }
        else
        {
            //Follow Path Here
            goalVelocity = (currentCheckpoint - transform.position).normalized * moveSpeed;
        }
    }

    #endregion

    #region vunerable
    private void Vunerable()
    {
        lerpVisuals();
        enableColliders(currentState);

        goalVelocity = (cloudObject.transform.position - transform.position) * vunerableSpeedMultiplier;

        if (!isVunerable)
        {
            Invoke("stopVunerable", timeToBeVunerable);
            isVunerable = true;
        }
    }

    void stopVunerable()
    {
        isVunerable = false;
        currentState = KingCloudState.MOVING;
    }

    #endregion

    #region waiting
    private void Wait()
    {
        goalVelocity = Vector3.zero;
        lerpVisuals();
        enableColliders(currentState);

        if (!isWaiting)
        {
            Invoke("stopWaiting", waitingTime);
            isWaiting = true;
        }
    }

    void stopWaiting()
    {
        isWaiting = false;
        currentState = KingCloudState.MOVING;
    }

    #endregion

    #region attack
    private void Attack()
    {
        lerpVisuals();
        enableColliders(currentState);

        if (!isAttacking)
        {
            StartCoroutine(chargeAttack());
        }
    }
    IEnumerator chargeAttack()
    {
        isAttacking = true;

        Vector3 playerPos = player.transform.position;
        playerPos.y = transform.position.y;

        Vector3 dirToPlayer = playerPos - transform.position;
        dirToPlayer.y = 0f;

        //update velocity

        float startTime = Time.time;

        while ((Time.time - startTime) < timeToChargeBack)
        {
            goalVelocity = -dirToPlayer.normalized * chargeBackSpeed;
            Debug.Log("going back " + startTime + ", " + Time.time + ", | " + (Time.time - startTime) + " < ? " + timeToChargeBack);
            yield return null;
        }

        //float dist = Vector3.Distance(transform.position, endPos);

        while (Vector3.Distance(transform.position, player.transform.position) >= playerProximity)
        {
            goalVelocity = (player.transform.position - transform.position).normalized * chargeSpeed;
            Debug.Log("going forward");
            yield return null;

        }

        //stay in direction for a bit longer
        yield return new WaitForSeconds(overstepDuration);

        goalVelocity = Vector3.zero;

        Debug.Log("compelted attack");

        yield return new WaitForSeconds(timeToReturnToMove);

        isAttacking = false;

        if (Random.Range(0f, 1f) <= chanceToGoVunerable)
        {
            currentState = KingCloudState.VUNERABLE;
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) > distanceToAttack)
            {
                currentState = KingCloudState.MOVING;
            }
        }
    }

    #endregion

    #region helpers

    Vector3 checkpointPos()
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        // Calculate the X and Z coordinates using polar coordinates
        float xPos = cloudObject.transform.position.x + radiusPatrol * Mathf.Cos(randomAngle);
        float zPos = cloudObject.transform.position.z + radiusPatrol * Mathf.Sin(randomAngle);

        // Create the position vector
        return new Vector3(xPos, transform.position.y, zPos);
    }

    void setLerpInfo(KingCloudState state)
    {
        List<float> info = new List<float>();
        switch (state)
        {
            case KingCloudState.VUNERABLE:
                info = vunerableTornadoGoalLerpInfo;
                break;
            case KingCloudState.ATTACKING:
                info = attackTornadoGoalLerpInfo;
                break;
            case KingCloudState.MOVING:
                info = movingTornadoGoalLerpInfo;
                break;
            case KingCloudState.WAITING:
                info = waitingTornadoGoalLerpInfo;
                break;
        }

        rotationLerper.amplitudeRotations = new Vector3(info[0], info[1], info[2]);
        rotationLerper.speed = info[3];
    }

    float getTargetDissolve(int i, KingCloudState state)
    {
        switch (state)
        {
            case KingCloudState.VUNERABLE:
                return vunerableTornadoGoalDissolves[i];
            case KingCloudState.ATTACKING:
                return attackTornadoGoalDissolves[i];
            case KingCloudState.MOVING:
                return movingTornadoGoalDissolves[i];
            case KingCloudState.WAITING:
                return waitingTornadoGoalDissolves[i];
        }

        return 0f;
        
    }

    Color getTargetColor(int i, KingCloudState state)
    {
        switch (state)
        {
            case KingCloudState.VUNERABLE:
                return vunerableTornadoGoalColors[i];
            case KingCloudState.ATTACKING:
                return attackTornadoGoalColors[i];
            case KingCloudState.MOVING:
                return movingTornadoGoalColors[i];
            case KingCloudState.WAITING:
                return waitingTornadoGoalColors[i];
        }

        return Color.white;

    }

    Vector3 getTargetScale(int i, KingCloudState state)
    {
        switch (state)
        {
            case KingCloudState.VUNERABLE:
                return vunerableTornadoGoalScales[i];
            case KingCloudState.ATTACKING:
                return attackTornadoGoalScales[i];
            case KingCloudState.MOVING:
                return movingTornadoGoalScales[i];
            case KingCloudState.WAITING:
                return waitingTornadoGoalScales[i];
        }

        return Vector3.zero;

    }

    Vector3 getTargetPosition(int i, KingCloudState state)
    {
        switch (state)
        {
            case KingCloudState.VUNERABLE:
                return vunerableTornadoGoalPositions[i];
            case KingCloudState.ATTACKING:
                return attackTornadoGoalPositions[i];
            case KingCloudState.MOVING:
                return movingTornadoGoalPositions[i];
            case KingCloudState.WAITING:
                return waitingTornadoGoalPositions[i];
        }

        return Vector3.zero;

    }

    float getTargetEmission(int i, KingCloudState state)
    {
        switch (state)
        {
            case KingCloudState.VUNERABLE:
                return vunerableTornadoGoalParticleEmissions[i];
            case KingCloudState.ATTACKING:
                return attackTornadoGoalParticleEmissions[i];
            case KingCloudState.MOVING:
                return movingTornadoGoalParticleEmissions[i];
            case KingCloudState.WAITING:
                return waitingTornadoGoalParticleEmissions[i];
        }

        return 0f;
    }

    void enableColliders(KingCloudState state)
    {
        List<Collider> colliders = new List<Collider>();

        switch (state)
        {
            case KingCloudState.VUNERABLE:
                colliders = vunerableColliders;
                break;
            case KingCloudState.ATTACKING:
                colliders = attackingColliders;
                break;
            case KingCloudState.MOVING:
                colliders = attackingColliders;
                break;
            case KingCloudState.WAITING:
                colliders = waitingColliders;
                break;
        }

        //disable all colliders
        foreach (Collider col in vunerableColliders)
        {
            col.enabled = false;
        }

        foreach (Collider col in attackingColliders)
        {
            col.enabled = false;
        }

        foreach (Collider col in waitingColliders)
        {
            col.enabled = false;
        }

        //enab;e right colliders
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
    }

    #endregion
}


public enum KingCloudState
{
    VUNERABLE,
    ATTACKING,
    MOVING,
    WAITING
}
