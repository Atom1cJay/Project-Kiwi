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

    private void Awake()
    {
        movementSettings = MovementSettingsSO.Instance;
        charCont = GetComponent<CharacterController>();
        mi = GetComponent<MovementInfo>();
        moveThisFrame = new Fall(GetComponent<MovementInputInfo>(), mi, movementSettings, 0);
    }

    void Update()
    {
        Vector3 dir = DirectionOfMovement();
        moveThisFrame.AdvanceTime();
        print(moveThisFrame.AsString());
        float speedThisFrame = moveThisFrame.GetHorizSpeedThisFrame();
        Vector3 horizMovement = dir * speedThisFrame;
        charCont.Move(horizMovement * Time.deltaTime);
        Vector3 vertMovement = Vector3.up * moveThisFrame.GetVertSpeedThisFrame();
        charCont.Move(vertMovement * Time.deltaTime);
        IMove next = moveThisFrame.GetNextMove();
        moveThisFrame = next;
    }

    /// <summary>
    /// Gives the direction of player movement based on the player's rotation
    /// and the slope they're standing on.
    /// </summary>
    private Vector3 DirectionOfMovement()
    {
        if (!moveThisFrame.AdjustToSlope()) return transform.forward;

        float horizAngleFaced = Mathf.Atan2(transform.forward.z, transform.forward.x);

        float xDelta = Mathf.Cos(horizAngleFaced);
        float zDelta = Mathf.Sin(horizAngleFaced);
        float yDelta = (xDelta * PlayerSlopeHandler.XDeriv) + (zDelta * PlayerSlopeHandler.ZDeriv);

        // Fixing the quirks of y movement
        if (yDelta > 0) yDelta = 0; // CharacterController will take care of ascension
        if (mi.TouchingGround()) yDelta -= Mathf.Abs(yDelta * movementSettings.StickToGroundMultiplier); // To keep player stuck to ground

        Vector3 dir = new Vector3(xDelta, yDelta, zDelta);
        if (dir.magnitude > 1) { dir = dir.normalized; }
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
