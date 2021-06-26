using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject meant to hold the global movement settings.
/// </summary>
[CreateAssetMenu]
public class MovementSettingsSO : ScriptableObject
{
    public float maxSpeed;
    public static MovementSettingsSO instance;

    void OnEnable()
    {
        if (!instance)
        {
            Debug.Log("HAHHAHAHAHHAHAHH");
            instance = this;
        }
    }
}
