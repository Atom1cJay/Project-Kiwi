using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a container and calculator for information relevant to
/// the input of the user.
/// </summary>
public class MovementInputInfo : MonoBehaviour
{
    [SerializeField] InputActionsHolder inputActionsHolder;
    [SerializeField] GameObject relevantCamera;

    [HideInInspector] public UnityEvent OnJump;
    [HideInInspector] public UnityEvent OnJumpCancelled;
    [HideInInspector] public UnityEvent OnDiveInput;
    [HideInInspector] public UnityEvent OnHorizBoostCharge;
    [HideInInspector] public UnityEvent OnHorizBoostRelease;
    [HideInInspector] public UnityEvent OnVertBoostCharge;
    [HideInInspector] public UnityEvent OnVertBoostRelease;
    [HideInInspector] public UnityEvent OnGroundPound;
    [HideInInspector] public UnityEvent OnGlide;
    [HideInInspector] public UnityEvent OnGlideRelease;

    private MovementSettingsSO movementSettings;

    bool inReverseCoyoteTime;
    float vertBoostTimeCharged;

    void Start()
    {
        movementSettings = MovementSettingsSO.Instance;
        inputActionsHolder.inputActions.Player.Jump.performed += _ => OnJump.Invoke();
        inputActionsHolder.inputActions.Player.Jump.canceled += _ => OnJumpCancelled.Invoke();
        inputActionsHolder.inputActions.Player.Boost.started += _ => OnHorizBoostCharge.Invoke();
        inputActionsHolder.inputActions.Player.VertBoost.started += _ => OnVertBoostCharge.Invoke();
        OnVertBoostCharge.AddListener(() => StartCoroutine("WaitForVertBoostRelease"));
        inputActionsHolder.inputActions.Player.Boost.canceled += _ => OnHorizBoostRelease.Invoke();
        inputActionsHolder.inputActions.Player.Dive.performed += _ => OnDiveInput.Invoke();
        inputActionsHolder.inputActions.Player.GroundPound.performed += _ => OnGroundPound.Invoke();
        inputActionsHolder.inputActions.Player.Glide.performed += _ => OnGlide.Invoke();
        inputActionsHolder.inputActions.Player.Glide.canceled += _ => OnGlideRelease.Invoke();
        OnJump.AddListener(() => StartCoroutine(WaitReverseCoyoteTime()));
    }

    IEnumerator WaitForVertBoostRelease()
    {
        vertBoostTimeCharged = 0;
        while (vertBoostTimeCharged < movementSettings.VertBoostMaxChargeTime
            && inputActionsHolder.inputActions.Player.VertBoost.ReadValue<float>() != 0)
        {
            vertBoostTimeCharged += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (vertBoostTimeCharged > movementSettings.VertBoostMaxChargeTime)
        {
            vertBoostTimeCharged = movementSettings.VertBoostMaxChargeTime;
        }
        OnVertBoostRelease.Invoke();
    }

    /// <summary>
    /// Gives the amount of time that the player last charged their vertical
    /// boost. Maxes out at the maximum amount of time that a vertical boost
    /// can be charged. Resets every time the player does the input for another
    /// boost charge.
    /// </summary>
    public float VertBoostTimeCharged()
    {
        return vertBoostTimeCharged;
    }

    /// <summary>
    /// Gives the InputActions instance being used to calculate
    /// input information on the player.
    /// </summary>
    public InputActions GetInputActions()
    {
        return inputActionsHolder.inputActions;
    }

    /// <summary>
    /// Gives the normalized horizontal movement input.
    /// </summary>
    public Vector2 GetHorizontalInput()
    {
        Vector2 rawInput = inputActionsHolder.inputActions.Player.Move.ReadValue<Vector2>();

        if (rawInput.magnitude > 1)
        {
            rawInput = rawInput.normalized;
        }

        return rawInput;
    }

    /// <summary>
    /// Gives the horizontal input with respect to the rotation of the player.
    /// For instance, if the player is pressing forward but the player is facing
    /// forward-right, an input of (cos45, sin45) will be given.
    /// </summary>
    public Vector2 GetRelativeHorizontalInput()
    {
        float difference = relevantCamera.transform.eulerAngles.y - transform.eulerAngles.y;
        float curHorizAngle = Mathf.Atan2(GetHorizontalInput().y, GetHorizontalInput().x);
        float curHorizMagnitude = GetHorizontalInput().magnitude;
        curHorizAngle -= difference * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(curHorizAngle) * curHorizMagnitude, Mathf.Sin(curHorizAngle) * curHorizMagnitude);
    }

    /// <summary>
    /// Gives the normalized horizontal movement input.
    /// </summary>
    public Vector2 GetCameraInput()
    {
        float rawInputX = inputActionsHolder.inputActions.Camera.HorizontalRotate.ReadValue<float>();
        float rawInputY = inputActionsHolder.inputActions.Camera.VerticalRotate.ReadValue<float>();
        Vector2 rawInput = new Vector2(rawInputX, rawInputY);
        if (rawInput.magnitude > 1)
        {
            rawInput = rawInput.normalized;
        }
        return rawInput;
    }

    /// <summary>
    /// Is the horizontal input dissonance high enough for an air reverse to be registered?
    /// </summary>
    public bool AirReverseInput()
    {
        return GetHorizDissonance() > movementSettings.DissonanceForAirReverse
            && GetHorizontalInput().magnitude != 0;
    }

    /// <summary>
    /// Is the horizontal input dissonance high enough for a hard turn to be registered?
    /// </summary>
    public bool HardTurnInput()
    {
        return GetHorizDissonance() > movementSettings.DissonanceForHardTurn;
    }

    /// <summary>
    /// Gives the distance (min = 0, max = PI) between the direction the player is facing and the
    /// direction of horizontal input
    /// </summary>
    public float GetHorizDissonance()
    {
        Vector2 rawInput = GetHorizontalInput();
        float camDirection = relevantCamera.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float directionFacing = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float inputDirection = Mathf.Atan2(rawInput.x, rawInput.y) + camDirection;
        float inputVsFacing = Mathf.PI - Mathf.Abs(Mathf.Abs((inputDirection - directionFacing) % (2 * Mathf.PI)) - Mathf.PI);
        return inputVsFacing;
    }

    /// <summary>
    /// With the camera angle taken into consideration, what angle is the
    /// horizontal input "pointing towards?" In radians.
    /// </summary>
    public float GetInputDirection()
    {
        Vector2 rawInput = GetHorizontalInput();
        float camDirection = relevantCamera.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        return Mathf.Atan2(rawInput.x, rawInput.y) + camDirection;
    }

    /// <summary>
    /// Has the player been holding the jump button a short enough time that a
    /// jump would be acceptable if the player landed on the ground now?
    /// </summary>
    /// <returns></returns>
    public bool InReverseCoyoteTime()
    {
        return inReverseCoyoteTime;
    }

    IEnumerator WaitReverseCoyoteTime()
    {
        float t = 0;
        inReverseCoyoteTime = true;

        while (t < movementSettings.ReverseCoyoteTime && inputActionsHolder.inputActions.Player.Jump.ReadValue<float>() > 0)
        {
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        inReverseCoyoteTime = false;
    }

    public bool PressingBoost()
    {
        return inputActionsHolder.inputActions.Player.Boost.ReadValue<float>() > 0;
    }
}
