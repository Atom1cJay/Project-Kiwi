using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementInputInfo))]
[RequireComponent(typeof(MoveExecuter))]
public class RotationMovement : MonoBehaviour
{
    private MoveExecuter me;
    private MovementInputInfo mii;

    private void Awake()
    {
        me = GetComponent<MoveExecuter>();
        mii = GetComponent<MovementInputInfo>();
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        DetermineRotation();
    }

    /// <summary>
    /// Appropriately rotates the player considering the state they're in
    /// </summary>
    /// <param name="rawInput">The input whose direction will be rotated towards</param>
    private void DetermineRotation()
    {
        IMoveImmutable curMove = me.GetCurrentMove();
        float rotationSpeed = curMove.GetRotationSpeed();

        if (curMove.RotationIsRelative())
        {
            transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);
        }
        else
        {
            if (mii.GetHorizontalInput().magnitude == 0) return; // Otherwise would trend forward
            Quaternion targetRotation = Quaternion.Euler(0, mii.GetInputDirection() * Mathf.Rad2Deg, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
