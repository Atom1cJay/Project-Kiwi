using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an instruction to the camera.
/// </summary>
[System.Serializable]
public abstract class ACameraInstruction : MonoBehaviour
{
    [SerializeField] protected float travelTime;
    [SerializeField] protected float postTravelTime;
    [SerializeField] protected bool isSmooth;
    [SerializeField] protected bool timeStopped;
    public static bool RunningInstructions { get; protected set; } // Static because for ALL instructions, either some are running or none are

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetDomain()
    {
        RunningInstructions = false;
    }

    /// <summary>
    /// Exeucte the instructions, on the given camera transform, and the
    /// given original position where the traveling should end.
    /// </summary>
    public abstract void RunInstructions(Transform c, Vector3 restartPos, Quaternion restartRot);

    /// <summary>
    /// How long will the instructions take to exeucte from this instruction?
    /// </summary>
    public abstract float GetTotalExecutionTime();
}
