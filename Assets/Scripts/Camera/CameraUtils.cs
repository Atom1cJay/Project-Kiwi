using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the specific details and intricacies of camera movement.
/// </summary>
public class CameraUtils : MonoBehaviour
{
    [SerializeField] private Transform target; // Object to look at / surround
    [SerializeField] private Transform player; // For rotation reference
    [SerializeField] private float radiusToTarget; // Distance from target
    [SerializeField] private float colliderRadius; // When something is between the player and the camera, the radius to determine the cam is touching it
    [SerializeField] private float minRadiusToTarget = 0.1f;
    [SerializeField] private float radiusDecreaseAtContact = 0.5f;
    [SerializeField] private float initHorizAngle = 0; // 0 = directly behind, 1.57 = to right. Initial value serialized.
    [SerializeField] private float initVertAngle = 0; // 0 = exactly aligned, 1.57 = on top. Initial value serialized.
    [SerializeField] private float vertAngleMin = 0f;
    [SerializeField] private float vertAngleMax = 1.57f;
    private float horizAngle;
    private float vertAngle;
    const int layerMaskNoPlayer = ~(1 << 9);
    enum CameraMode
    {
        AroundPlayer, // Will stay a certain (possibly inhibited) radius around the player
        FollowingInstructions // Will not move relative to the player, can be ordered to move anywhere
    }
    CameraMode camMode = CameraMode.AroundPlayer;

    private void Awake()
    {
        horizAngle = initHorizAngle;
        vertAngle = initVertAngle;
        CenterAroundPlayer();
    }

    /// <summary>
    /// Rotates the camera by the given amounts.
    /// </summary>
    /// <param name="horizAmount">The amount of horizontal rotation (degrees)</param>
    /// <param name="vertAmount">The amount of vertical rotation (degrees)</param>
    public void RotateBy(float horizAmount, float vertAmount)
    {
        if (camMode != CameraMode.AroundPlayer)
        {
            //Debug.LogError("Can't rotate camera since it's not in AroundPlayer mode.");
            return;
        }
        horizAngle += horizAmount;
        vertAngle += vertAmount;
        vertAngle = Mathf.Clamp(vertAngle, vertAngleMin, vertAngleMax);
    }

    /// <summary>
    /// Takes the given amount of time to rotate to the back of the player.
    /// </summary>
    public void RotateToTargetY(float time)
    {
        if (camMode != CameraMode.AroundPlayer)
        {
            return;
        }
        StopCoroutine("RotateTowardsY");
        StartCoroutine("RotateTowardsY", time);
    }

    /// <summary>
    /// Coroutine for moving the rotation to the back of the player.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator RotateTowardsY(float time)
    {
        float initialY = -transform.eulerAngles.y * Mathf.Deg2Rad;
        float goalY = -player.eulerAngles.y * Mathf.Deg2Rad;
        Quaternion angle1 = Quaternion.Euler(0, initialY * Mathf.Rad2Deg, 0);
        Quaternion angle2 = Quaternion.Euler(0, goalY * Mathf.Rad2Deg, 0);
        float rotationSpeed = Mathf.Abs(Quaternion.Angle(angle1, angle2)) / time;
        float elapsed = 0;
        while (elapsed < time)
        {
            Quaternion shit = Quaternion.Euler(0, -transform.eulerAngles.y, 0);
            Quaternion shit2 = Quaternion.Euler(0, -player.eulerAngles.y, 0);
            Quaternion newRot = Quaternion.RotateTowards(shit, shit2, rotationSpeed * Time.deltaTime);
            horizAngle = newRot.eulerAngles.y * Mathf.Deg2Rad;
            CenterAroundPlayer();
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void LateUpdate()
    {
        if (camMode == CameraMode.AroundPlayer)
        {
            CenterAroundPlayer();
        }
    }

    /// <summary>
    /// Based on the current horizontal / vertical angle, as well as the position/radius
    /// of the target, positions and angles the camera appropriately
    /// </summary>xw
    private void CenterAroundPlayer()
    {
        // Take horizontal angle into account
        float actualRadius = GetActualRadius();
        float relativeX = actualRadius * Mathf.Sin(horizAngle);
        float relativeZ = actualRadius * -Mathf.Cos(horizAngle);
        // Take vertical angle into account
        relativeX *= Mathf.Cos(vertAngle);
        relativeZ *= Mathf.Cos(vertAngle);
        float relativeY = actualRadius * Mathf.Sin(vertAngle);
        // Change position
        transform.position = target.position + new Vector3(relativeX, relativeY, relativeZ);
        transform.LookAt(target);
    }

    /// <summary>
    /// What radius should I be around the player, taking into account any walls the camera might be
    /// behind (which I should not clip through)?
    /// </summary>
    /// <returns></returns>
    /// 
    private float GetActualRadius()
    {
        // TODO this might be a problem in the first frame where the player and camera are potentailly far away
        RaycastHit[] hits = Physics.RaycastAll(target.position, (transform.position - target.position).normalized, radiusToTarget, layerMaskNoPlayer);
        // Check that there's a hit between player and camera
        if (hits.Length > 0)
        {
            RaycastHit[] reverseHits = Physics.RaycastAll(transform.position, (target.position - transform.position).normalized, (target.position - transform.position).magnitude, layerMaskNoPlayer);

            if (reverseHits.Length < hits.Length)
            {
                return Mathf.Max(GetShortestHitDistance(hits) - radiusDecreaseAtContact, minRadiusToTarget); // Inhibited radius
            }
            else
            {
                return radiusToTarget; // Regular radius
            }
        }
        else
        {
            return radiusToTarget; // Regular radius
        }
    }

    float GetShortestHitDistance(RaycastHit[] hits)
    {
        RaycastHit shortest = hits[0];

        for (int i = 1; i < hits.Length; i++)
        {
            if (hits[i].distance < shortest.distance)
            {
                shortest = hits[i];
            }
        }

        return shortest.distance;
    }

    /// <summary>
    /// Moves the camera closer to the back of the player by the ratio given.
    /// </summary>
    public void RotateToBackBy(float ratio)
    {
        if (camMode != CameraMode.AroundPlayer)
        {
            //Debug.LogError("Can't rotate camera since it's not in AroundPlayer mode.");
            return;
        }
        Quaternion angle1 = Quaternion.Euler(0, horizAngle * Mathf.Rad2Deg, 0);
        Quaternion angle2 = Quaternion.Euler(0, -player.eulerAngles.y, 0);
        Quaternion newRot = Quaternion.RotateTowards(angle1, angle2, Quaternion.Angle(angle1, angle2) * ratio);
        horizAngle = newRot.eulerAngles.y * Mathf.Deg2Rad;
    }

    /// <summary>
    /// Moves the camera closer to the given vertical angle (degrees) by the ratio given.
    /// </summary>
    public void RotateToVertAngle(float ratio, float angle)
    {
        if (camMode != CameraMode.AroundPlayer)
        {
            //Debug.LogError("Can't rotate camera since it's not in AroundPlayer mode.");
            return;
        }
        Quaternion angle1 = Quaternion.Euler(vertAngle * Mathf.Rad2Deg, 0, 0);
        Quaternion angle2 = Quaternion.Euler(angle, 0, 0);
        Quaternion newRot = Quaternion.RotateTowards(angle1, angle2, Quaternion.Angle(angle1, angle2) * ratio);
        vertAngle = newRot.eulerAngles.x * Mathf.Deg2Rad;
    }

    /// <summary>
    /// Unpacks and follows the given list of instructions, returning back to the initial
    /// position if the instructions indicate a position exactly at the origin.
    /// </summary>
    /// <param name="instructions"></param>
    public void HandleInstructions(ACameraInstruction instructions)
    {
        if (camMode == CameraMode.FollowingInstructions)
        {
            Debug.LogError("Cannot accept instructions; some are already running!");
            return;
        }
        camMode = CameraMode.FollowingInstructions;
        instructions.RunInstructions(transform, transform.position);
        StartCoroutine(WaitForInstructions(instructions));
    }

    /// <summary>
    /// Waits for some specific instructions to complete, and then goes back into AroundPlayer
    /// mode.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private IEnumerator WaitForInstructions(ACameraInstruction i)
    {
        float timeElapsed = 0;
        float totalTime = i.GetTotalExecutionTime();

        while (timeElapsed < totalTime)
        {
            timeElapsed += IndependentTime.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        camMode = CameraMode.AroundPlayer;
    }
}
