using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides important knowledge based on the slope the player is currently standing on.
/// </summary>
[RequireComponent(typeof(MovementInfo))]
public class PlayerSlopeHandler : MonoBehaviour
{
    /// <summary>
    /// The largest angle the player is allowed to walk on normally
    /// </summary>
    private static readonly float maxSlopeAngle = 45;
    /// <summary>
    /// The angle of the slope the player is currently colliding with.
    /// </summary>
    public static float AngleOfSlope { get; private set; }
    /// <summary>
    /// For every positive x unit you move, you go up by this amount.
    /// </summary>
    public static float XDeriv { get; private set; }
    /// <summary>
    /// For every positive z unit you move, you go up by this amount.
    /// </summary>
    public static float ZDeriv { get; private set; }
    /// <summary>
    /// Is the player on a slope steeper than it can handle?
    /// </summary>
    public static bool BeyondMaxAngle { get; private set; }

    private CollisionDetector groundDetector;

    private void Start()
    {
        groundDetector = GetComponent<MovementInfo>().GetGroundDetector();
    }

    // Obtains the normal of the platform the player is currently on
    // (Is called whenever the player moves in a way which ends with it touching something)
    // EFFECT: Modifies AngleOfSlope to refer to the correct angle
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Make sure that this is a valid slope (the platform is angled)
        RaycastHit rchit;
        int layerMask = ~(1 << 9);
        Debug.DrawRay(hit.point + Vector3.up, Vector3.down);
        if (Physics.Raycast(hit.point + Vector3.up, Vector3.down, out rchit, Mathf.Infinity, layerMask)) // layer mask?
        {
            XDeriv = -rchit.normal.x;
            ZDeriv = -rchit.normal.z;
            AngleOfSlope = GetAngleOfSlope(rchit.normal);
            DetermineIfBeyondAngle();
            print(rchit.normal);
        }
    }

    /// <summary>
    /// Determines whether or not the player is beyond the max angle. Has
    /// different standards depending on the current max angle because
    /// otherwise, a 45 degree angle would constantly go on and off.
    /// </summary>
    private void DetermineIfBeyondAngle()
    {
        if (!BeyondMaxAngle)
        {
            BeyondMaxAngle = maxSlopeAngle < AngleOfSlope;
        }
        else
        {
            BeyondMaxAngle = maxSlopeAngle < AngleOfSlope + 1;
        }
    }

    /// <summary>
    /// Gets the angle, in degrees, of the slope the player is standing on right now.
    /// </summary>
    /// <param name="normal">The normal of the slope the player is standing on</param>
    /// <returns></returns>
    private float GetAngleOfSlope(Vector3 normal)
    {
        float xSq = (Mathf.Pow(Mathf.Abs(normal.x), 2));
        float zSq = (Mathf.Pow(Mathf.Abs(normal.z), 2));
        float sin = Mathf.Sqrt(xSq + zSq);
        return Mathf.Asin(sin) * Mathf.Rad2Deg;
    }
}
