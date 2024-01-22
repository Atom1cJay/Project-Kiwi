using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRaft : AMovingPlatform
{
    enum Phase
    {
        Rising,
        Floating,
        Sinking
    }

    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    [SerializeField] float riseSinkYMvmt;
    [SerializeField] float riseSinkTime;
    [SerializeField] float floatTime; // Time moving from endRisePos to startSinkPos
    float totalTime;
    Phase curPhase;
    float relativePhaseProgress; // From 0 (starting) to 1 (ending)
    float relativeProgress; // From 0 (starting) to 1 (ending)

    private void Start()
    {
        if (startPos.position.y != endPos.position.y)
        {
            Debug.LogError("Start Pos Y and End Pos Y are different. This may not be intentional.");
        }
        GoToStartTransform();
        totalTime = (riseSinkTime * 2) + floatTime;
        curPhase = Phase.Rising;
        relativeProgress = 0;
        relativePhaseProgress = 0;
    }

    void GoToStartTransform()
    {
        transform.position = startPos.position;
        Vector3 moveVector = endPos.position - startPos.position;
        float moveVectorDir = Mathf.Atan2(moveVector.z, moveVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, moveVectorDir + 90, 0);
    }

    void HandleHorizontalMovement()
    {
        relativeProgress = Mathf.Clamp(relativeProgress + (Time.deltaTime / totalTime), 0, 1);
        Vector3 btwnPos = Vector3.Lerp(startPos.position, endPos.position, Mathf.SmoothStep(0, 1, relativeProgress));
        transform.position = new Vector3(btwnPos.x, transform.position.y, btwnPos.z);
        if (relativeProgress >= 1)
        {
            relativeProgress = 0;
        }
    }

    void HandleRising()
    {
        relativePhaseProgress = Mathf.Clamp(relativePhaseProgress + (Time.deltaTime / riseSinkTime), 0, 1);
        float y = startPos.position.y + Mathf.SmoothStep(0, riseSinkYMvmt, relativePhaseProgress);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        if (relativePhaseProgress >= 1)
        {
            curPhase = Phase.Floating;
            relativePhaseProgress = 0;
        }
    }

    void HandleFloating()
    {
        relativePhaseProgress = Mathf.Clamp(relativePhaseProgress + (Time.deltaTime / floatTime), 0, 1);
        if (relativePhaseProgress >= 1)
        {
            curPhase = Phase.Sinking;
            relativePhaseProgress = 0;
        }
    }

    void HandleSinking()
    {
        relativePhaseProgress = Mathf.Clamp(relativePhaseProgress + (Time.deltaTime / riseSinkTime), 0, 1);
        float y = startPos.position.y + Mathf.SmoothStep(riseSinkYMvmt, 0, relativePhaseProgress);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        if (relativePhaseProgress >= 1)
        {
            Destroy(gameObject);
        }
    }

    public override void Translate()
    {
        HandleHorizontalMovement();
        switch (curPhase)
        {
            case Phase.Rising:
                HandleRising();
                break;
            case Phase.Floating:
                HandleFloating();
                break;
            case Phase.Sinking:
                HandleSinking();
                break;
        }
    }

    public override void Rotate()
    {
        // No rotation
    }
}