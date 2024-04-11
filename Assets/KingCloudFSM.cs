using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCloudFSM : MonoBehaviour
{
    [Header("Moving Vars")]
    [SerializeField] float moveSpeed;


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

    [Header("Attacking Vars")]
    [SerializeField] float damage;

    [Header("Mesh /Visual References")]
    [SerializeField] List<MeshRenderer> tornadoParts;
    [SerializeField] List<ParticleSystem> particleParts;
    [SerializeField] GameObject visualParent;
    [SerializeField] float lerpSpeed;


    public KingCloudState currentState;

    bool alive;
    // Start is called before the first frame update
    void Start()
    {
        alive = true;
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

    private void Move()
    {
        lerpVisuals();
    }

    private void Attack()
    {
        lerpVisuals();
    }

    private void Vunerable()
    {
        lerpVisuals();
    }
    private void Wait()
    {
        lerpVisuals();
    }
    void lerpVisuals()
    {
        for (int i = 0; i < tornadoParts.Count; i++)
        {
            //mesh stuff
            MeshRenderer mr = tornadoParts[i];
            float currentDisolve = mr.material.GetFloat("_Dissolve");
            mr.material.SetFloat("_Dissolve", Mathf.Lerp(currentDisolve, getTargetDissolve(i, currentState), Time.deltaTime * lerpSpeed));
            Color currentColor = mr.material.GetColor("_Color");
            mr.material.SetColor("_Color", Color.Lerp(currentColor, getTargetColor(i, currentState), Time.deltaTime * lerpSpeed));

            //positioning stuff
            GameObject meshObject = mr.gameObject;
            meshObject.transform.localPosition = Vector3.Lerp(meshObject.transform.localPosition, getTargetPosition(i, currentState), Time.deltaTime * lerpSpeed);
            meshObject.transform.localScale = Vector3.Lerp(meshObject.transform.localScale, getTargetScale(i, currentState), Time.deltaTime * lerpSpeed);
        }

        for (int i = 0; i < particleParts.Count; i++)
        {

            ParticleSystem particleSystem = particleParts[i];

            //particles

            ParticleSystem.EmissionModule emission = particleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(emission.rateOverTime.constant, getTargetEmission(i, currentState), Time.deltaTime * lerpSpeed);
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
    }


    #region helpers
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

    #endregion
}


public enum KingCloudState
{
    VUNERABLE,
    ATTACKING,
    MOVING,
    WAITING
}
