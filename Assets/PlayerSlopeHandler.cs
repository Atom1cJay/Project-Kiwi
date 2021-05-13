using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides important knowledge based on the slope the player is currently standing on.
/// </summary>
public class PlayerSlopeHandler : MonoBehaviour
{
    /// <summary>
    /// For every positive x unit you move, you go up by this amount.
    /// </summary>
    public static float X_DERIV { get; private set; }
    /// <summary>
    /// For every positive z unit you move, you go up by this amount.
    /// </summary>
    public static float Z_DERIV { get; private set; }

    // Obtains the normal of the platform the player is currently on
    // (Is called whenever the player moves in a way which ends with it touching something)
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        X_DERIV = -hit.normal.x; // For every (pos) x unit you move, you go up this amount
        Z_DERIV = -hit.normal.z; // For every (pos) z unit you move, you go up this amount
//        print("X: " + X_DERIV + ", Z: " + Z_DERIV);
    }
}
