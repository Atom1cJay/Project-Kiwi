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
    [SerializeField] protected bool isSmooth; // TODO make smoothness matter
    [SerializeField] protected bool timeStopped;

    /// <summary>
    /// Exeucte the instructions, on the given camera transform, and the
    /// given original position where the traveling should end.
    /// </summary>
    public abstract void RunInstructions(Transform c, Vector3 restartPos);

    /// <summary>
    /// How long will the instructions take to exeucte from this instruction?
    /// </summary>
    public abstract float GetTotalExecutionTime();
}
