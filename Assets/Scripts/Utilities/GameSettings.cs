using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles any meta settings related to the game 
/// </summary>
public class GameSettings : MonoBehaviour
{
    [SerializeField] private int targetFPS;

    /*
    private void Awake()
    {
        Application.targetFrameRate = targetFPS;
    }
    */
}
