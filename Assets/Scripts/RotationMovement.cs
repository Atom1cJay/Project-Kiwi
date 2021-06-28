using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementInputInfo))]
[RequireComponent(typeof(MovementMaster))]
[RequireComponent(typeof(MoveExecuter))]
public class RotationMovement : MonoBehaviour
{
    private MovementMaster mm;
    private MovementInfo mi;
    private MoveExecuter me;

    private void Awake()
    {
        mi = GetComponent<MovementInfo>();
        mm = GetComponent<MovementMaster>();
        me = GetComponent<MoveExecuter>();
        mm.mm_OnHardTurnEnd.AddListener(OnHardTurnEnd);
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
        // Get basic direction info
        Vector2 rawInput = mm.GetHorizontalInput();
        float camDirection = mm.GetRelevantCamera().transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float inputDirection = Mathf.Atan2(rawInput.x, rawInput.y) + camDirection;

        if (!mm.IsInHardTurn())
        {
            float rotationSpeed = DetermineRotationSpeed();
            if (rawInput.magnitude == 0) return;
            Quaternion targetRotation = Quaternion.Euler(0, inputDirection * Mathf.Rad2Deg, 0);
            bool underInstantRotSpeed = mi.currentSpeedHoriz <= MovementSettingsSO.Instance.InstantRotationSpeed;
            transform.rotation =
                underInstantRotSpeed && !inAirBoostOrCharge()
                ? targetRotation
                :
                Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Decides what the rotation should be based on the player's movement state
    /// </summary>
    /// <returns></returns>
    float DetermineRotationSpeed()
    {
        return me.rotationThisFrame();
    }

    bool inAirBoostOrCharge()
    {
        return mm.InAirBoost() || mm.InAirBoostCharge() || mm.InVertAirBoostCharge();
    }

    /// <summary>
    /// Changes the rotation appropriately for the end of a hard turn.
    /// </summary>
    private void OnHardTurnEnd()
    {
        Vector2 rawInput = mm.GetHorizontalInput();
        if (rawInput.magnitude == 0) return;
        float camDirection = mm.GetRelevantCamera().transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float inputDirection = Mathf.Atan2(rawInput.x, rawInput.y) + camDirection;
        Quaternion targetRotation = Quaternion.Euler(0, inputDirection * Mathf.Rad2Deg, 0);
        transform.rotation = targetRotation;
    }
}
