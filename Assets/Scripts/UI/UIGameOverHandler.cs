using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Calls for the death screen to be spawned when the player dies.
/// The actual calling of the death screen is handled by a UIControlSystem.
/// </summary>
public class UIGameOverHandler : MonoBehaviour
{
    [SerializeField] PlayerHealth pe;
    public UnityEvent OnDeathScreenSpawnCalled;

    void Start()
    {
        pe.onDeath.AddListener(() => OnDeathScreenSpawnCalled.Invoke());
    }
}
