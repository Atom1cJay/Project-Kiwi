using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides important knowledge based on the slope the player is currently standing on.
/// </summary>
[RequireComponent(typeof(MovementInfo))]
public class PlayerSlopeHandler : MonoBehaviour
{
    public static bool GroundInProximity;
    public static float DistanceOfGroundInProximity;
    private static readonly int layerMask = ~(1 << 9);
    /// <summary>
    /// The angle of the slope the player is currently colliding with.
    /// </summary>
    private static float AngleOfSlope;
    /// <summary>
    /// For every positive x unit you move, you go up by this amount.
    /// </summary>
    public static float XDeriv { get; private set; }
    /// <summary>
    /// For every positive z unit you move, you go up by this amount.
    /// </summary>
    public static float ZDeriv { get; private set; }
    /// <summary>
    /// Is the player on a slope steep enough that a slide should take place?
    /// </summary>
    public static bool ShouldSlide { get; private set; }
    /// <summary>
    /// The contact point which the normal used in angle calculations is
    /// reflected off of.
    /// </summary>
    public static Vector3 AngleContactPoint { get; private set; }

    [SerializeField] float maxHeightOfContactPoint;
    /// <summary>
    /// Slope considered too steep for regular movement
    /// </summary>
    [SerializeField] float maxSlopeAngle = 60;
    /// <summary>
    /// After the player is on a max slope, what is the angle which gets the player back to regular movement
    /// </summary>
    [SerializeField] float recoveryAngle = 50;
    [SerializeField] float maxAngleForProximity = 45;
    [SerializeField] float lengthOfNearestGroundRay;
    [SerializeField] CollisionDetector groundProximityDetector;
    [SerializeField] CharacterController charCont;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetDomain()
    {
        GroundInProximity = false;
        AngleOfSlope = 0;
        XDeriv = 0;
        ZDeriv = 0;
        ShouldSlide = false;
        AngleContactPoint = Vector3.zero;
    }

    private void LateUpdate()
    {
        UpdateGroundProximityInfo();
    }

    /// <summary>
    /// Update the information about whether the ground is in proximity
    /// and, if so, how far it is from the player. Can be good to call right
    /// before checking whether the ground is in proximity.
    /// </summary>
    public void UpdateGroundProximityInfo()
    {
        DetectIfGroundInProximity();
    }

    private void DetectIfGroundInProximity()
    {
        GroundInProximity = groundProximityDetector.Colliding();
        print(GroundInProximity);
        /*
        GroundInProximity = false;
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up, Vector3.down);
        bool couldTouchGround = Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 1 + lengthOfNearestGroundRay, layerMask);
        //bool couldTouchGround = Physics.Raycast(charCont.bounds.min, Vector3.down, out hit, 1 + lengthOfNearestGroundRay, layerMask);
        GroundInProximity = couldTouchGround && GetAngleOfSlope(hit.normal) < maxAngleForProximity;
        DistanceOfGroundInProximity = GroundInProximity ? hit.distance - 1 : -1;
        //print(DistanceOfGroundInProximity);
        */
    }

    // Obtains the normal of the platform the player is currently on
    // (Is called whenever the player moves in a way which ends with it touching something)
    // EFFECT: Modifies AngleOfSlope to refer to the correct angle
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.point.y > hit.controller.bounds.min.y + maxHeightOfContactPoint)
        {
            return;
        }
        RaycastHit rchit;
        // Make sure that this is a valid slope (the platform is angled)
        if (Physics.Raycast(hit.point + Vector3.up, Vector3.down, out rchit, Mathf.Infinity, layerMask)) // layer mask?
        {
            // Register slope
            XDeriv = -Mathf.Tan(Mathf.Asin(rchit.normal.x));
            ZDeriv = -Mathf.Tan(Mathf.Asin(rchit.normal.z));
            AngleOfSlope = GetAngleOfSlope(rchit.normal);
            ShouldSlide = DetermineIfShouldSlide(hit.gameObject);
            AngleContactPoint = rchit.point;
        }
    }

    /// <summary>
    /// Determines whether or not the player is beyond the max angle. Has
    /// different standards depending on the current max angle because
    /// otherwise, a 45 degree angle would constantly go on and off.
    /// </summary>
    private bool DetermineIfShouldSlide(GameObject slopeObj)
    {
        if (!slopeObj.CompareTag("Slidable"))
        {
            return false;
        }
        if (!ShouldSlide)
        {
            return maxSlopeAngle < AngleOfSlope;
        }
        else
        {
            return AngleOfSlope > recoveryAngle;
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
