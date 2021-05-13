using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A version of CollisionDetector which can only exist in one instance at a time. Used
/// to detect the ground.
/// </summary>
public class PlayerGroundCollisionDetector : CollisionDetector
{
    private static PlayerGroundCollisionDetector _instance;
    public static PlayerGroundCollisionDetector Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        if (this == _instance)
        {
            _instance = null;
        }
    }
}
