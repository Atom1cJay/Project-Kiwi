using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of time stats independent of Time.timeScale.
/// </summary>
public class IndependentTime : MonoBehaviour
{
    public static float deltaTime;
    private float prevTime; // Time elapsed (s) prev frame

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetDomain()
    {
        deltaTime = 0;
    }

    void LateUpdate()
    {
        deltaTime = Time.realtimeSinceStartup - prevTime;
        prevTime = Time.realtimeSinceStartup;
    }
}
