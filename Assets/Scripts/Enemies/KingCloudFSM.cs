using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCloudFSM : MonoBehaviour
{
    [Header("Moving Vars")]
    [SerializeField] float moveSpeed;
    [SerializeField] float radiusPatrol;
    [SerializeField] float distanceToAttack;
    [SerializeField] float verticalDistanceToAttack;
    [SerializeField] float distanceToNextPoint;

    [Header("Attacking Vars")]
    [SerializeField] float timeToChargeBack;
    [SerializeField] float chargeBackSpeed;
    [SerializeField] float playerProximity;
    [SerializeField] float chargeMaxPlayerProximity;
    [SerializeField] float overstepDuration;
    [SerializeField] float chargeSpeed;
    [SerializeField] float initialChargeSpeed;
    [SerializeField] float chargeForwardDuration;
    [SerializeField] float pauseAfterAttack;
    [SerializeField] float chanceToGoVunerable;
    [SerializeField] Yeeter ourDamager;
    [SerializeField] float bufferTime;
    [SerializeField] float missMultiplier;
    [SerializeField] float attackMaxDuration;
    [SerializeField] float chargeMaxDuration;

    [Header("Vunerable Vars")]
    [SerializeField] float timeToBeVunerable;
    [SerializeField] float vunerableSpeedMultiplier;
    [SerializeField] float lockedMoveDuration;
    [SerializeField] ParticleSystem takeDamageParticles;

    [Header("Waiting Vars")]
    [SerializeField] float waitingTime;

    [Header("Start Up Vars")]
    [SerializeField] float startUpDistance;
    [SerializeField] float startWaitingTime;
    [SerializeField] GameObject startUpTextObject;


    [Header("Ending Vars")]
    [SerializeField] float endingWaitingTime;
    [SerializeField] float fadeOutEndingTime;
    [SerializeField] GameObject endingTextObject;
    [SerializeField] List<MeshRenderer> tornadoPartsEnding;

    [Header("Game Objects")]
    [SerializeField] List<GameObject> movingGameObjects;
    [SerializeField] List<GameObject> attackingGameObjects;
    [SerializeField] List<GameObject> vunerableGameObjects;
    [SerializeField] List<GameObject> waitingGameObjects;

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
    [SerializeField] List<float> startUpTornadoGoalLerpInfo;

    [Header("Mesh /Visual References")]
    [SerializeField] List<MeshRenderer> tornadoParts;
    [SerializeField] List<ParticleSystem> particleParts;
    [SerializeField] LerpRotation rotationLerper;
    [SerializeField] GameObject visualParent;
    [SerializeField] float moveLerpSpeed;
    [SerializeField] float visualLerpSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] GameObject cloudObject;
    [SerializeField] BossBattleController levelController;

    [Header("Health Info")]
    [SerializeField] int health;
    [SerializeField] GameObject canvasParent;
    [SerializeField] GameObject healthBarObject;
    [SerializeField] List<GameObject> stageObjects;

    [Header("Sound Effects")]
    [SerializeField] Sound chargeSFX;
    [SerializeField] Sound releaseSFX;
    [SerializeField] Sound damageSFX;
    [SerializeField] Sound ambienceSFX;

    bool lockRotation = false;

    public KingCloudState currentState;

    Vector3 currentCheckpoint;
    public Vector3 velocity;
    public Vector3 goalVelocity;

    bool alive;
    bool isAttacking;
    bool isVunerable;
    bool isWaiting;
    bool isStarted;

    bool didAttackHit = false;
    bool lockedInMove = false;

    float attackTimeStart;

    int missCount;
    
    GameObject player;
    GameObject currentStageObject;
    Vector3 ir;


    // Start is called before the first frame update
    void Start()
    {
        //set up data
        ir = transform.eulerAngles;
        isAttacking = false;
        isVunerable = false;
        isWaiting = false;
        isStarted = false;
        player = GameObject.FindGameObjectWithTag("Player");
        currentCheckpoint = checkpointPos();
        alive = true;

        //set up SFX
        AudioMasterController.instance.PlaySound(ambienceSFX, transform);

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
        currentStageObject = stageObjects[health - 1];

        //Start up

        currentState = KingCloudState.STARTUP;

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
                case KingCloudState.STARTUP:
                    StartUp();
                    break;

            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Debug.Log("good job, boss defeated");

        StartCoroutine(endBossBattle());
    }

    IEnumerator endBossBattle()
    {
        //Ending logic
        currentState = KingCloudState.STARTUP;
        endingTextObject.SetActive(true);
        isStarted = false;

        float t = Time.time;
        while (Time.time - t < endingWaitingTime)
        {
            currentState = KingCloudState.STARTUP;
            StartUp();
            yield return null;
        }

         //stop lerping visuals and moving
         isStarted = true;

        //fade out tornados
        t = Time.time;

        //get initial dissolves
        List<float> dissolves = new List<float>();
        List<float> additionalDissolves = new List<float>();

        for (int i = 0; i < tornadoParts.Count; i++)
            dissolves.Add(tornadoParts[i].material.GetFloat("_Dissolve"));
        for (int i = 0; i < tornadoPartsEnding.Count; i++)
            additionalDissolves.Add(tornadoPartsEnding[i].material.GetFloat("_Dissolve"));

        while (Time.time - t < fadeOutEndingTime)
        {
            float pct = (Time.time - t) / fadeOutEndingTime;

            for (int i = 0; i < tornadoParts.Count; i++)
            {
                //mesh stuff
                MeshRenderer mr = tornadoParts[i];
                mr.material.SetFloat("_Dissolve", Mathf.Lerp(dissolves[i], 1f, pct));
            }

            for (int i = 0; i < tornadoPartsEnding.Count; i++)
            {
                //mesh stuff
                MeshRenderer mr = tornadoPartsEnding[i];
                mr.material.SetFloat("_Dissolve", Mathf.Lerp(additionalDissolves[i], 1f, pct));
            }

            yield return null;
        }

        //disable stage objects

        levelController.lerpCloudAlpha(currentStageObject, null);
        levelController.endBossBattle();

        Destroy(gameObject);
    }
    void Update()
    {
        if (currentState == KingCloudState.MOVING || currentState == KingCloudState.ATTACKING)
        {
            velocity = Vector3.Lerp(velocity, goalVelocity, Time.deltaTime * moveLerpSpeed);
            velocity = new Vector3(velocity.x, 0f, velocity.z);
            transform.position += velocity * Time.deltaTime;
        }
    }

    #region damage

    public void takeDamage()
    {
        health -= 1;
        takeDamageParticles.Play();

        AudioMasterController.instance.PlaySound(damageSFX);

        if (health == 0)
        {
            alive = false;
        }
        else
        {
            Destroy(canvasParent.transform.GetChild(0).gameObject);

            levelController.lerpCloudAlpha(currentStageObject, stageObjects[health - 1]);
            currentStageObject = stageObjects[health - 1];

            //do cutscene

            CancelInvoke("stopVunerable");
            isVunerable = false;
            currentState = KingCloudState.WAITING;
        }
    }

    public void bufferDamage()
    {
        Debug.Log("buffer damage");
        ourDamager.isActivated = false;
        didAttackHit = true;
        Invoke("endBuffer", bufferTime);
    }

    void endBuffer()
    {
        ourDamager.isActivated = true;
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

        setGameObjects(currentState);
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

        Vector2 XZtransformPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 XZplayerPosition = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 XZcheckpointPosition = new Vector2(currentCheckpoint.x, currentCheckpoint.z);
        missCount = 0;


        //rotate visuals
        Quaternion q = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * rotateSpeed);
        Vector3 v = q.eulerAngles;
        transform.eulerAngles = new Vector3(ir.x, v.y, ir.z);

        if (Vector2.Distance(XZtransformPosition, XZplayerPosition) <= distanceToAttack && 
            Mathf.Abs(transform.position.y - player.transform.position.y) <= verticalDistanceToAttack &&
            !lockedInMove)
        {
            currentState = KingCloudState.ATTACKING;
        }
        else if (Vector3.Distance(XZtransformPosition, XZcheckpointPosition) <= distanceToNextPoint)
        {
            currentCheckpoint = checkpointPos();
        }
        else
        {
            //Follow Path Here
            goalVelocity = (currentCheckpoint - transform.position).normalized * moveSpeed;
        }
    }

    void unlockedMove()
    {
        lockedInMove = false;
    }

    #endregion

    #region vunerable
    private void Vunerable()
    {
        lerpVisuals();
        enableColliders(currentState);
        missCount = 0;

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
        lockedInMove = true;
        Invoke("unlockedMove", lockedMoveDuration);
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
            attackTimeStart = Time.time;
        }
    }
    IEnumerator chargeAttack()
    {
        //set up vars
        isAttacking = true;
        didAttackHit = false;

        Vector3 XZtransformPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 XZplayerPosition = new Vector3(player.transform.position.x, 0f, player.transform.position.z);

        Vector3 dirToPlayer = XZplayerPosition - XZtransformPosition;
        dirToPlayer.y = 0f;

        Quaternion q;
        Vector3 v;

        float startTime = Time.time;

        //charge SFX
        AudioMasterController.instance.PlaySound(chargeSFX, transform);

        //charge start up
        while ((Time.time - startTime) < timeToChargeBack)
        {
            goalVelocity = -dirToPlayer.normalized * chargeBackSpeed;

            //rotate visuals
            q = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dirToPlayer), Time.deltaTime * rotateSpeed);
            v = q.eulerAngles;
            transform.eulerAngles = new Vector3(ir.x, v.y, ir.z);
            //Debug.Log("going back " + startTime + ", " + Time.time + ", | " + (Time.time - startTime) + " < ? " + timeToChargeBack);
            yield return null;
        }

        //charge release
        AudioMasterController.instance.PlaySound(releaseSFX, transform);

        //charge forward initially straight ahead if not too close
        startTime = Time.time;
        while ((Time.time - startTime) < chargeForwardDuration && Vector3.Distance(XZtransformPosition, XZplayerPosition) >= playerProximity)
        {
            XZtransformPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            XZplayerPosition = new Vector3(player.transform.position.x, 0f, player.transform.position.z);
            goalVelocity = dirToPlayer.normalized * initialChargeSpeed;
            yield return null;
        }

        //lock onto player
        
        XZtransformPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        XZplayerPosition = new Vector3(player.transform.position.x, 0f, player.transform.position.z);

        startTime = Time.time;

        while (Vector3.Distance(XZtransformPosition, XZplayerPosition) >= playerProximity && //if super close to player
            (startTime - Time.time <= chargeMaxDuration && Vector3.Distance(XZtransformPosition, XZplayerPosition) >= chargeMaxPlayerProximity)) //hit max duration and close enough
        {
            XZtransformPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            XZplayerPosition = new Vector3(player.transform.position.x, 0f, player.transform.position.z);
            goalVelocity = (XZplayerPosition - XZtransformPosition).normalized * chargeSpeed;

            //rotate visuals
            q = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * rotateSpeed);
            v = q.eulerAngles;
            transform.eulerAngles = new Vector3(ir.x, v.y, ir.z);

            //Debug.Log("going forward  + " + Vector3.Distance(XZtransformPosition, XZplayerPosition));
            yield return null;

        }

        //stay in direction for a bit longer
        yield return new WaitForSeconds(overstepDuration);

        goalVelocity = Vector3.zero;

        Debug.Log("compelted attack");

        //after attack pause
        startTime = Time.time;
        while ((Time.time - startTime) < pauseAfterAttack)
        {
            //rotate visuals
            q = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), Time.deltaTime * rotateSpeed);
            v = q.eulerAngles;
            transform.eulerAngles = new Vector3(ir.x, v.y, ir.z);
            yield return null;
        }

        //reset var
        isAttacking = false;

        //if attack hit go to move
        if (didAttackHit)
        {
            currentState = KingCloudState.MOVING;

            lockedInMove = true;
            Invoke("unlockedMove", lockedMoveDuration);

        }
        //if missed enough or too long go vunerable
        else if (Random.Range(0f, 1f) - (missMultiplier * missCount) <= chanceToGoVunerable || (Time.time - attackTimeStart >= attackMaxDuration))
        {
            currentState = KingCloudState.VUNERABLE;
        }
        else
        {
            missCount++;
            if (Vector3.Distance(transform.position, player.transform.position) > distanceToAttack &&
                Mathf.Abs(transform.position.y - player.transform.position.y) > verticalDistanceToAttack)
            {
                currentState = KingCloudState.MOVING;
            }
        }
    }

    #endregion

    #region startup

    void StartUp()
    {
        if (!isStarted)
        {
            Vector2 XZtransformPosition = new Vector2(transform.position.x, transform.position.z);
            Vector2 XZplayerPosition = new Vector2(player.transform.position.x, player.transform.position.z);

            lerpVisuals();

            //rotation
            Quaternion q = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), Time.deltaTime * rotateSpeed);
            Vector3 v = q.eulerAngles;

            transform.eulerAngles = new Vector3(ir.x, v.y, ir.z);

            goalVelocity = player.transform.position - transform.position;
        }
    }

    public void StartBossBattleTimer()
    {
        isStarted = true;
        Invoke("StartBossBattle", startWaitingTime);
    }

    void StartBossBattle()
    {
        Vector2 XZtransformPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 XZplayerPosition = new Vector2(player.transform.position.x, player.transform.position.z);
        missCount = 0;

        Debug.Log(" pos  " + Vector2.Distance(XZtransformPosition, XZplayerPosition) + " | " + Mathf.Abs(transform.position.y - player.transform.position.y));

        if (Vector2.Distance(XZtransformPosition, XZplayerPosition) <= distanceToAttack &&
            Mathf.Abs(transform.position.y - player.transform.position.y) <= verticalDistanceToAttack)
        {
            currentState = KingCloudState.ATTACKING;
        }
        else
        {
            currentState = KingCloudState.MOVING;
        }
        startUpTextObject.SetActive(false);
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
            case KingCloudState.STARTUP:
                info = startUpTornadoGoalLerpInfo;
                break;
            case KingCloudState.WAITING:
                info = waitingTornadoGoalLerpInfo;
                break;
        }

        rotationLerper.amplitudeRotations = new Vector3(info[0], info[1], info[2]);
        rotationLerper.speed = info[3];
    }

    void setGameObjects(KingCloudState state)
    {
        List<GameObject> gameList = new List<GameObject>();
        switch (state)
        {
            case KingCloudState.VUNERABLE:
                gameList = vunerableGameObjects;
                break;
            case KingCloudState.ATTACKING:
                gameList = attackingGameObjects;
                break;
            case KingCloudState.MOVING:
                gameList = movingGameObjects;
                break;
            case KingCloudState.WAITING:
                gameList = waitingGameObjects;
                break;
        }

        foreach (GameObject g in vunerableGameObjects)
            g.SetActive(false);

        foreach (GameObject g in attackingGameObjects)
            g.SetActive(false);

        foreach (GameObject g in movingGameObjects)
            g.SetActive(false);

        foreach (GameObject g in waitingGameObjects)
            g.SetActive(false);

        foreach (GameObject g in gameList)
            g.SetActive(true);


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
            case KingCloudState.STARTUP:
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
            case KingCloudState.STARTUP:
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
            case KingCloudState.STARTUP:
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
            case KingCloudState.STARTUP:
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
            case KingCloudState.STARTUP:
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
                colliders = new List<Collider>();
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
    WAITING,
    STARTUP,
    DEATH
}
