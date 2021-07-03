using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MoveExecuter : MonoBehaviour
{
    IMove moveThisFrame;
    CharacterController charCont;
    MovementInfo mi;
    MovementSettingsSO movementSettings;

    private void Start()
    {
        movementSettings = MovementSettingsSO.Instance;
        charCont = GetComponent<CharacterController>();
        mi = GetComponent<MovementInfo>();
        moveThisFrame = new Fall(GetComponent<MovementInputInfo>(), mi, movementSettings, 0, false);
    }

    void Update()
    {
        moveThisFrame.AdvanceTime();
        Vector2 horizMovement = moveThisFrame.GetHorizSpeedThisFrame();
        Vector3 dir = DirectionOfMovement(horizMovement);
        Vector3 horizMovementAdjusted = dir * horizMovement.magnitude;
        Vector3 vertMovement = Vector3.up * moveThisFrame.GetVertSpeedThisFrame();
        charCont.Move((vertMovement + horizMovementAdjusted) * Time.deltaTime);
        IMove next = moveThisFrame.GetNextMove();
        moveThisFrame = next;
    }

    /// <summary>
    /// Gives the direction of player movement based on the player's horizontal
    /// movement vector and the slope they're standing on.
    /// </summary>
    private Vector3 DirectionOfMovement(Vector2 horizMovement)
    {
        if (!moveThisFrame.AdjustToSlope()) return new Vector3(horizMovement.x, 0, horizMovement.y).normalized;

        float horizAngleFaced = Mathf.Atan2(horizMovement.y, horizMovement.x);

        float xDelta = Mathf.Cos(horizAngleFaced);
        float zDelta = Mathf.Sin(horizAngleFaced);
        float yDelta = (xDelta * PlayerSlopeHandler.XDeriv) + (zDelta * PlayerSlopeHandler.ZDeriv);

        // Fixing the quirks of y movement
        if (yDelta > 0) yDelta = 0; // CharacterController will take care of ascension

        Vector3 dir = new Vector3(xDelta, yDelta, zDelta);
        return dir;
    }

    /// <summary>
    /// Provides immutable access to the move which is currently taking place.
    /// </summary>
    public IMoveImmutable GetCurrentMove()
    {
        return moveThisFrame;
    }
}
