using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An instruction to the camera to go to a specific position.
/// </summary>
[System.Serializable]
public class CameraInstruction : ACameraInstruction
{
    [SerializeField] Transform goalTransform;
    [SerializeField] ACameraInstruction next;
    [SerializeField] bool canSkipToLeaf;
    [SerializeField] bool runningThis;
    [SerializeField] float lerpSpeedForForce;
    [SerializeField] bool lerpToGoal;

    private void Awake()
    {
        if (travelTime < 0 || postTravelTime < 0)
        {
            Debug.LogError("Travel / post travel time must not be negative.");
        }
        if (goalTransform == null)
        {
            Debug.LogError("No goal transform given.");
        }
        if (next == null)
        {
            Debug.LogError("This camera instruction doesn't have a next instruction." +
                "Attach a leaf.");
        }
    }

    // DO NOT CALL THIS DIRECTLY FROM A UNITYEVENT OR SOMETHING.
    // Call the CameraUtils method to make this process happen.
    public override void RunInstructions(Transform c, Vector3 initPos, Quaternion restartRot)
    {
        RunningInstructions = true;
        runningThis = true;
        onInstructionsStart.Invoke();
        StartCoroutine(RunInstructionsSequence(c, initPos, restartRot));
    }

    IEnumerator RunInstructionsSequence(Transform c, Vector3 restartPos, Quaternion restartRot)
    {
        TimescaleHandler.setPausedForCameraTransition(timeStopped);

        // Travel
        float timeElapsed = 0;
        Vector3 startingPos = c.position;
        Quaternion startingRot = c.rotation;

        while (timeElapsed < travelTime)
        {
            timeElapsed += IndependentTime.deltaTime;
            float timeRatio = timeElapsed / travelTime;

            if (lerpToGoal)
            {
                c.position = Vector3.Lerp(c.position, goalTransform.position, lerpSpeedForForce * Time.deltaTime);
                c.rotation = Quaternion.Lerp(c.rotation, goalTransform.rotation, lerpSpeedForForce * Time.deltaTime);
            }
            else if (isSmooth)
            {
                c.position = new Vector3(
                    Mathf.SmoothStep(startingPos.x, goalTransform.position.x, timeRatio),
                    Mathf.SmoothStep(startingPos.y, goalTransform.position.y, timeRatio),
                    Mathf.SmoothStep(startingPos.z, goalTransform.position.z, timeRatio));
                c.rotation = Quaternion.Lerp(startingRot, goalTransform.rotation, Mathf.SmoothStep(0, 1, timeRatio));
            }
            else
            {
                c.position = Vector3.Lerp(startingPos, goalTransform.position, timeRatio);
                c.rotation = Quaternion.Lerp(startingRot, goalTransform.rotation, timeRatio);
            }

            yield return new WaitForEndOfFrame();
        }

        // Pause
        c.position = goalTransform.position;
        c.rotation = goalTransform.rotation;
        timeElapsed = 0;

        while (timeElapsed < postTravelTime)
        {
            timeElapsed += IndependentTime.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Next in sequence
        runningThis = false;
        next.RunInstructions(c, restartPos, restartRot);
    }

    void Update()
    {
        if (runningThis && canSkipToLeaf && Input.GetKeyDown(KeyCode.Space)) // TODO controller skippable
        {
            StopAllCoroutines();
            runningThis = false;
            GetLeaf().RunInstructions(transform, transform.position, transform.rotation);
        }
    }

    /// <summary>
    /// How long will the instructions take to exeucte from this instruction?
    /// </summary>
    public override float GetTotalExecutionTime()
    {
        return travelTime + postTravelTime + next.GetTotalExecutionTime();
    }

    public override CameraInstructionLeaf GetLeaf()
    {
        return next.GetLeaf();
    }
}
