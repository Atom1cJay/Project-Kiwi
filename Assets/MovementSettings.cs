using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing constants relevant to horiontal/vertical movement
/// </summary>
public class MovementSettings : MonoBehaviour
{
    [Header("Horizontal Settings")]
    public float defaultMaxSpeedX;
    public float runSensitivityX;
    public float runGravityX;
    public float runGravityXOverTopSpeed;
    public float airSensitivityX;
    public float airGravityX;
    public float airGravityXOverTopSpeed;
    public float tjAirSensitivityX;
    public float tjInputGravityX;
    public float hardTurnGravityX;
    public float horizBoostSpeedX;
    public float horizBoostChargeGravityX;
    public float vertBoostChargeGravityX;
    public float groundBoostMaxSpeedX;
    public float groundBoostSensitivityX;
    public float groundBoostGravityX;
    public float diveSpeedX;
    public float airReverseSensitivityX;
    public float airReverseGravityX;

    [Header("Jump Settings")]
    public float jumpInitVel;
    public float jumpInitGravity;
    public float jumpMaxUncancelledGravity;
    public float jumpMaxCancelledGravity;
    public float jumpVelMultiplierAtCancel;
    public float jumpUncancelledGravityIncreaseRate;
    public float jumpCancelledGravityIncreaseRate;

    [Header("Triple Jump Settings")]
    public float tjInitJumpVel;
    public float tjInitGravity;
    public float tjUncancelledMaxGravity;
    public float tjCancelledMaxGravity;
    public float tjVelocityMultiplier;
    public float tjGravityIncRate;
    public float tjGravityIncRateAtCancel;

    [Header("Boost Settings")]
    public float horizBoostGravity;
    public float horizBoostChargeGravity;
    public float horizBoostEndGravity;
    public float vertBoostMinVel;
    public float vertBoostMaxVel;
    public float vertBoostGravity;

    [Header("Dive Settings")]
    public float diveInitVel;
    public float diveGravity;

    [Header("Misc. Vertical")]
    public float defaultGravity;
    public float stickToGroundMultiplier;
}
