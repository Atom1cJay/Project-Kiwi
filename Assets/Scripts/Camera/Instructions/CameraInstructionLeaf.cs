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

    public override void RunInstructions(Transform c, Vector3 initPos)
    {
        StartCoroutine(RunInstructionsProcess(c, initPos));
    }

    private IEnumerator RunInstructionsProcess(Transform c, Vector3 initPos)
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
                    Mathf.SmoothStep(startingPos.x, initPos.x, timeRatio),
                    Mathf.SmoothStep(startingPos.y, initPos.y, timeRatio),
                    Mathf.SmoothStep(startingPos.z, initPos.z, timeRatio));
            }
            else
            {
                c.position = Vector3.Lerp(startingPos, initPos, timeRatio);
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
    }

    public override float GetTotalExecutionTime()
    {
        return travelTime;
    }
}
