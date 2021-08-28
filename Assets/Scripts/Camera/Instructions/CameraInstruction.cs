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

    public override void RunInstructions(Transform c, Vector3 initPos)
    {
        StartCoroutine(RunInstructionsSequence(c, initPos));
    }

    IEnumerator RunInstructionsSequence(Transform c, Vector3 restartPos)
    {
        TimescaleHandler.setPausedForCameraTransition(timeStopped);

        // Travel
        float timeElapsed = 0;
        Vector3 startingPos = c.position;

        while (timeElapsed < travelTime)
        {
            timeElapsed += IndependentTime.deltaTime;
            float timeRatio = timeElapsed / travelTime;

            if (isSmooth)
            {
                c.position = new Vector3(
                    Mathf.SmoothStep(startingPos.x, goalTransform.position.x, timeRatio),
                    Mathf.SmoothStep(startingPos.y, goalTransform.position.y, timeRatio),
                    Mathf.SmoothStep(startingPos.z, goalTransform.position.z, timeRatio));
            }
            else
            {
                c.position = Vector3.Lerp(startingPos, goalTransform.position, timeRatio);
            }

            yield return new WaitForEndOfFrame();
        }

        // Pause
        c.position = goalTransform.position;
        timeElapsed = 0;

        while (timeElapsed < postTravelTime)
        {
            timeElapsed += IndependentTime.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Next in sequence
        next.RunInstructions(c, restartPos);
    }

    /// <summary>
    /// How long will the instructions take to exeucte from this instruction?
    /// </summary>
    public override float GetTotalExecutionTime()
    {
        return travelTime + postTravelTime + next.GetTotalExecutionTime();
    }
}
