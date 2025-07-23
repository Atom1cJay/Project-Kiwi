using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player rotation, based on the currently active IMove.
/// </summary>
[RequireComponent(typeof(MovementInputInfo))]
[RequireComponent(typeof(MoveExecuter))]
public class RotationMovement : MonoBehaviour
{
    private MovementInputInfo mii;

    void Awake()
    {
        mii = GetComponent<MovementInputInfo>();
    }

    /// <summary>
    /// Appropriately rotates the player considering the IMove they're in.
    /// </summary>
    /// <param name="rawInput">The input whose direction will be rotated towards</param>
    public void DetermineRotation()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        IMoveImmutable curMove = MoveExecuter.instance.GetCurrentMove();
        RotationInfo rotInfo = curMove.GetRotationInfo();
        float rotationSpeed = rotInfo.speed;
        if (mii.GetHorizontalInput().magnitude == 0) return; // Otherwise would trend forward
        Quaternion targetRotation = Quaternion.Euler(0, mii.GetInputDirection() * Mathf.Rad2Deg, 0);
        // Choose how to rotate depending on rotationInfo
        if (rotInfo.speedRelativeToDistance)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Quaternion.Angle(transform.rotation, targetRotation) * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Rotates the player around the y axis by the given amount.
    /// </summary>
    public void RotateExtra(float amount)
    {
        transform.Rotate(0, amount, 0);
    }
}
