using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HorizontalMovement))]
[RequireComponent(typeof(VerticalMovement))]
[RequireComponent(typeof(MovementMaster))]
[RequireComponent(typeof(MovementSettings))]
[RequireComponent(typeof(CharacterController))]
public class MoveExecuter : MonoBehaviour
{
    IMove moveThisFrame;
    // TODO for speedThisFrame, maybe get rid of the one in movementinfo
    // (and that class in general)
    public float speedThisFrame { get; private set; }
    CharacterController charCont;
    MovementMaster mm;
    MovementSettings ms;
    MovementInfo mi;

    private void Awake()
    {
        charCont = GetComponent<CharacterController>();
        mm = GetComponent<MovementMaster>();
        ms = GetComponent<MovementSettings>();
        mi = GetComponent<MovementInfo>();
        moveThisFrame = new Fall(mm, ms, GetComponent<MovementInputInfo>(), mi);
    }

    void Update()
    {
        Vector3 dir = directionOfMovement();
        float speed = moveThisFrame.GetHorizSpeedThisFrame();
        mi.currentSpeed = speed;
        this.speedThisFrame = speed;
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
    private Vector3 directionOfMovement()
    {
        if (mm.IsJumping()) return transform.forward;

        float horizAngleFaced = Mathf.Atan2(transform.forward.z, transform.forward.x);

        float xDelta = Mathf.Cos(horizAngleFaced);
        float zDelta = Mathf.Sin(horizAngleFaced);
        float yDelta = (xDelta * PlayerSlopeHandler.XDeriv) + (zDelta * PlayerSlopeHandler.ZDeriv);

        // Fixing the quirks of y movement
        if (yDelta > 0) yDelta = 0; // CharacterController will take care of ascension
        if (mm.IsOnGround()) yDelta -= Mathf.Abs(yDelta * ms.stickToGroundMultiplier); // To keep player stuck to ground

        Vector3 dir = new Vector3(xDelta, yDelta, zDelta);
        if (dir.magnitude > 1) { dir = dir.normalized; }
        return dir;
    }

    /// <summary>
    /// Returns the currently active move in string form. Strings are all
    /// lowercase and have no spaces in them, but other than that, they're
    /// identical to their class names.
    /// </summary>
    /// <returns></returns>
    public string currentMoveAsString()
    {
        return moveThisFrame.ToString();
    }
}
