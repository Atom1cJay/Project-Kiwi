using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject meant to hold constants related to player movement.
/// A singleton can be obtained in order to get the constants.
/// </summary>
[CreateAssetMenu]
public class MovementSettingsSO : ScriptableObject
{
    /// Serialized, private variables
    [Header("Horizontal Settings")]
    [SerializeField] float maxSpeed;
    [SerializeField] float runSensitivityX;
    [SerializeField] float runGravityX;
    [SerializeField] float runGravityXOverTopSpeed;
    [SerializeField] float airSensitivityX;
    [SerializeField] float airGravityX;
    [SerializeField] float airGravityXOverTopSpeed;
    [SerializeField] float tjAirSensitivityX;
    [SerializeField] float tjInputGravityX;
    [SerializeField] float hardTurnGravityX;
    [SerializeField] float horizBoostSpeedX;
    [SerializeField] float horizBoostChargeGravityX;
    [SerializeField] float vertBoostChargeGravityX;
    [SerializeField] float groundBoostMaxSpeedX;
    [SerializeField] float groundBoostSensitivityX;
    [SerializeField] float groundBoostGravityX;
    [SerializeField] float diveSpeedX;
    [SerializeField] float airReverseSensitivityX;
    [SerializeField] float airReverseGravityX;

    [Header("Jump Settings")]
    [SerializeField] float jumpInitVel;
    [SerializeField] float jumpInitGravity;
    [SerializeField] float jumpMaxUncancelledGravity;
    [SerializeField] float jumpMaxCancelledGravity;
    [SerializeField] float jumpVelMultiplierAtCancel;
    [SerializeField] float jumpUncancelledGravityIncrease;
    [SerializeField] float jumpCancelledGravityIncrease;

    [Header("Triple Jump Settings")]
    [SerializeField] float tjInitJumpVel;
    [SerializeField] float tjInitGravity;
    [SerializeField] float tjUncancelledMaxGravity;
    [SerializeField] float tjCancelledMaxGravity;
    [SerializeField] float tjVelocityMultiplier;
    [SerializeField] float tjGravityIncRate;
    [SerializeField] float tjGravityIncRateAtCancel;

    [Header("Boost Settiungs")]
    [SerializeField] float horizBoostGravity;
    [SerializeField] float horizBoostChargeGravity;
    [SerializeField] float horizBoostEndGravity;
    [SerializeField] float horizBoostMaxChargeTime;
    [SerializeField] float horizBoostMaxTime;
    [SerializeField] float vertBoostMinVel;
    [SerializeField] float vertBoostMaxVel;
    [SerializeField] float vertBoostGravity;
    [SerializeField] float vertBoostMaxChargeTime;
    [SerializeField] float vertBoostMaxTime;

    [Header("Dive Settings")]
    [SerializeField] float diveInitVel;
    [SerializeField] float diveGravity;

    [Header("Misc. Vertical")]
    [SerializeField] float defaultGravity;
    [SerializeField] float stickToGroundMultiplier;

    [Header("Misc.")]
    [SerializeField] float hardTurnTime;

    /// public, readonly variables
    public float MaxSpeed { get { return maxSpeed; } }
    public float RunSensitivityX { get { return runSensitivityX; } }
    public float RunGravityX { get { return runGravityX; } }
    public float RunGravityXOverTopSpeed { get { return runGravityXOverTopSpeed; } }
    public float AirSensitivityX { get { return airSensitivityX; } }
    public float AirGravityX { get { return airGravityX; } }
    public float AirGravityXOverTopSpeed { get { return airGravityXOverTopSpeed; } }
    public float TjAirSensitivityX { get { return tjAirSensitivityX; } }
    public float TjInputGravityX { get { return tjInputGravityX; } }
    public float HardTurnGravityX { get { return hardTurnGravityX; } }
    public float HorizBoostSpeedX { get { return horizBoostSpeedX; } }
    public float HorizBoostChargeGravityX { get { return horizBoostChargeGravityX; } }
    public float VertBoostChargeGravityX { get { return vertBoostChargeGravityX; } }
    public float GroundBoostMaxSpeedX { get { return groundBoostMaxSpeedX; } }
    public float GroundBoostSensitivityX { get { return groundBoostSensitivityX; } }
    public float GroundBoostGravityX { get { return groundBoostGravityX; } }
    public float DiveSpeedX { get { return diveSpeedX; } }
    public float AirReverseSensitivityX { get { return airReverseSensitivityX; } }
    public float AirReverseGravityX { get { return airReverseGravityX; } }

    public float JumpInitVel { get { return jumpInitVel; } }
    public float JumpInitGravity { get { return jumpInitGravity; } }
    public float JumpMaxUncancelledGravity { get { return jumpMaxUncancelledGravity; } }
    public float JumpMaxCancelledGravity { get { return jumpMaxCancelledGravity; } }
    public float JumpVelMultiplierAtCancel { get { return jumpVelMultiplierAtCancel; } }
    public float JumpUncancelledGravityIncrease { get { return jumpUncancelledGravityIncrease; } }
    public float JumpCancelledGravityIncrease { get { return jumpCancelledGravityIncrease; } }

    public float TjInitJumpVel { get { return tjInitJumpVel; } }
    public float TjInitGravity { get { return tjInitGravity; } }
    public float TjUncancelledMaxGravity { get { return tjUncancelledMaxGravity; } }
    public float TjCancelledMaxGravity { get { return tjCancelledMaxGravity; } }
    public float TjVelocityMultiplier { get { return tjVelocityMultiplier; } }
    public float TjGravityIncRate { get { return tjGravityIncRate; } }
    public float TjGravityIncRateAtCancel { get { return tjGravityIncRateAtCancel; } }

    public float HorizBoostGravity { get { return horizBoostGravity; } }
    public float HorizBoostChargeGravity { get { return horizBoostChargeGravity; } }
    public float HorizBoostEndGravity { get { return horizBoostEndGravity; } }
    public float HorizBoostMaxChargeTime { get { return horizBoostMaxChargeTime; } }
    public float HorizBoostMaxTime { get { return horizBoostMaxTime; } }
    public float VertBoostMinVel { get { return vertBoostMinVel; } }
    public float VertBoostMaxVel { get { return vertBoostMaxVel; } }
    public float VertBoostGravity { get { return vertBoostGravity; } }
    public float VertBoostMaxChargeTime { get { return vertBoostMaxChargeTime; } }
    public float VertBoostMaxTime { get { return vertBoostMaxTime; } }

    public float DiveInitVel { get { return diveInitVel; } }
    public float DiveGravity { get { return diveGravity; } }

    public float DefaultGravity { get { return defaultGravity; } }
    public float StickToGroundMultiplier { get { return maxSpeed; } }

    public float HardTurnTime { get { return hardTurnTime; } }

    public static MovementSettingsSO instance;

    void OnEnable()
    {
        if (!instance)
        {
            instance = this;
        }
    }
}
