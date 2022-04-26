using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The end of the instructions, in which the camera must return to the player.
/// </summary>
[System.Serializable]
public class CameraInstructionLeaf : ACameraInstruction
{
    private void Awake()
    {
        if (travelTime < 0)
        {
            Debug.LogError("Travel time can't be negative.");
        }
    }

    public override void RunInstructions(Transform c, Vector3 initPos, Quaternion restartRot)
    {
        StartCoroutine(RunInstructionsProcess(c, initPos, restartRot));
    }

    private IEnumerator RunInstructionsProcess(Transform c, Vector3 initPos, Quaternion restartRot)
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

            if (isSmooth)
            {
                c.position = new Vector3(
                    Mathf.SmoothStep(startingPos.x, initPos.x, timeRatio),
                    Mathf.SmoothStep(startingPos.y, initPos.y, timeRatio),
                    Mathf.SmoothStep(startingPos.z, initPos.z, timeRatio));
                c.rotation = Quaternion.Lerp(startingRot, restartRot, Mathf.SmoothStep(0, 1, timeRatio));
            }
            else
            {
                c.position = Vector3.Lerp(startingPos, initPos, timeRatio);
                c.rotation = Quaternion.Lerp(startingRot, restartRot, timeRatio);
            }

            yield return new WaitForEndOfFrame();
        }

        // Pause
        timeElapsed = 0;

        while (timeElapsed < postTravelTime)
        {
            timeElapsed += IndependentTime.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // At end
        TimescaleHandler.setPausedForCameraTransition(false);
        RunningInstructions = false;
    }

    public override float GetTotalExecutionTime()
    {
        return travelTime + postTravelTime;
    }
}
