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
    [SerializeField] float jumpGroundableTimer;
    [SerializeField] float jumpInitVel;
    [SerializeField] float jumpInitGravity;
    [SerializeField] float jumpMaxUncancelledGravity;
    [SerializeField] float jumpMaxCancelledGravity;
    [SerializeField] float jumpVelMultiplierAtCancel;
    [SerializeField] float jumpUncancelledGravityIncrease;
    [SerializeField] float jumpCancelledGravityIncrease;
    [SerializeField] float jumpVelocityOfNoReturn;
    [SerializeField] float jumpMinVel;

    [Header("Coyote Time Settings")]
    [SerializeField] float reverseCoyoteTime;
    [SerializeField] float coyoteTime;

    [Header("Hard Turn Settings")]
    [SerializeField] float dissonanceForHardTurn;
    [SerializeField] float hardTurnMinSpeed;
    [SerializeField] float hardTurnTime;

    [Header("Triple Jump Settings")]
    [SerializeField] float tjInitJumpVel;
    [SerializeField] float tjInitGravity;
    [SerializeField] float tjUncancelledMaxGravity;
    [SerializeField] float tjCancelledMaxGravity;
    [SerializeField] float tjVelocityMultiplier;
    [SerializeField] float tjGravityIncRate;
    [SerializeField] float tjGravityIncRateAtCancel;

    [Header("Triple Jump Activator Settings")]
    [SerializeField] float tjMaxBreakSpeed;
    [SerializeField] float tjMinHorizInputMagnitude; 
    [SerializeField] float tjMaxTimeBtwnJumps;
    [SerializeField] float tjMinJumpTime;
    [SerializeField] float tjMaxJumpTime;
    [SerializeField] float tjMaxDissonance;

    [Header("Boost Settings")]
    [SerializeField] float horizBoostMinGravity;
    [SerializeField] float horizBoostMaxGravity;
    [SerializeField] float horizBoostChargeGravity;
    [SerializeField] float horizBoostChargeRotationSpeed;
    [SerializeField] float horizBoostEndGravity;
    [SerializeField] float horizBoostMaxChargeTime;
    [SerializeField] float horizBoostMaxTime;
    [SerializeField] float horizBoostRotation;
    [SerializeField] float horizBoostAirReverseGravity;
    [SerializeField] float horizBoostNonAirReverseGravity;
    [SerializeField] float horizBoostMinActivationX;
    [SerializeField] float horizBoostMaxActivationX;
    [SerializeField] float vertBoostMinVel;
    [SerializeField] float vertBoostMaxVel;
    [SerializeField] float vertBoostGravity;
    [SerializeField] float vertBoostMaxChargeTime;
    [SerializeField] float vertBoostMaxTime;
    [SerializeField] float vertBoostRotationSpeed;
    [SerializeField] float vertBoostMaxSpeedX;

    [Header("Ground Pound Settings")]
    [SerializeField] float gpSuspensionTime;
    [SerializeField] float gpDownSpeed;

    [Header("Dive Settings")]
    [SerializeField] float diveInitVel;
    [SerializeField] float diveGravity;
    [SerializeField] float diveRecoveryTime;

    [Header("Misc. Vertical")]
    [SerializeField] float defaultGravity;
    [SerializeField] float stickToGroundMultiplier;

    [Header("Misc. Horizontal")]
    [SerializeField] float dissonanceForAirReverse;
    [SerializeField] float airReverseMinActivationSpeed;

    [Header("Rotation Settings")]
    [SerializeField] float instantRotationSpeed;
    [SerializeField] float groundRotationSpeed;
    [SerializeField] float airRotationSpeed;
    [SerializeField] float boostAftermathRotationSpeed;
    [SerializeField] float diveRotationSpeed;
    [SerializeField] float groundBoostRotationSpeed;

    [Header("Slide Settings")]
    [SerializeField] float slideForce;
    [SerializeField] float slideMaxSpeed;
    [SerializeField] float slideRecoveryPace;

    [Header("Glide Settings")]
    [SerializeField] float glideRotationSpeed;
    [SerializeField] float glideRotationSpeedSensitivity;
    [SerializeField] float glideRotationSpeedGravity;
    [SerializeField] float glideTiltSensitivity;
    [SerializeField] float glideAirLoss;
    [SerializeField] float glideHorizontalSpeed;
    [SerializeField] float glideMaxHorizontalSpeed;
    [SerializeField] float glideVerticalSpeed;
    [SerializeField] float glideMaxVerticalSpeed;
    [SerializeField] Vector2 glideDriftMovementVector;
    [SerializeField] float glideEnterDriftSpeed;
    [SerializeField] float glideTimeToStartDrift;
    [SerializeField] float glideDriftDuration;
    [SerializeField] float glideDriftBufferDuration;
    [SerializeField] float glideWeight;
    [SerializeField] float glideMaxTilt;
    [SerializeField] float glideMaxRoll;
    [SerializeField] float glideGravity;
    [SerializeField] float glideCameraAdjustRatio;
    [SerializeField] float glideNonControlTime;
    [SerializeField] float glideMinCamAngleX;
    [SerializeField] float glideMaxCamAngleX;
    [SerializeField] float glideJumpSpeed;
    [SerializeField] float glideJumpTime;
    [SerializeField] float glideXSensitivity;
    [SerializeField] float glideXGravity;

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

    public float JumpGroundableTimer { get { return jumpGroundableTimer; } }
    public float JumpInitVel { get { return jumpInitVel; } }
    public float JumpInitGravity { get { return jumpInitGravity; } }
    public float JumpMaxUncancelledGravity { get { return jumpMaxUncancelledGravity; } }
    public float JumpMaxCancelledGravity { get { return jumpMaxCancelledGravity; } }
    public float JumpVelMultiplierAtCancel { get { return jumpVelMultiplierAtCancel; } }
    public float JumpUncancelledGravityIncrease { get { return jumpUncancelledGravityIncrease; } }
    public float JumpCancelledGravityIncrease { get { return jumpCancelledGravityIncrease; } }
    public float JumpVelocityOfNoReturn { get { return jumpVelocityOfNoReturn;  } }
    public float JumpMinVel { get { return jumpMinVel; } }

    public float ReverseCoyoteTime { get { return reverseCoyoteTime; } }
    public float CoyoteTime { get { return coyoteTime; } }

    public float DissonanceForHardTurn { get { return dissonanceForHardTurn; } }
    public float HardTurnMinSpeed { get { return hardTurnMinSpeed; } }
    public float HardTurnTime { get { return hardTurnTime; } }

    public float TjInitJumpVel { get { return tjInitJumpVel; } }
    public float TjInitGravity { get { return tjInitGravity; } }
    public float TjUncancelledMaxGravity { get { return tjUncancelledMaxGravity; } }
    public float TjCancelledMaxGravity { get { return tjCancelledMaxGravity; } }
    public float TjVelocityMultiplier { get { return tjVelocityMultiplier; } }
    public float TjGravityIncRate { get { return tjGravityIncRate; } }
    public float TjGravityIncRateAtCancel { get { return tjGravityIncRateAtCancel; } }

    public float TjMaxBreakSpeed { get { return tjMaxBreakSpeed; } }
    public float TjMinHorizInputMagnitude { get { return tjMinHorizInputMagnitude; } }
    public float TjMaxTimeBtwnJumps { get { return tjMaxTimeBtwnJumps; } }
    public float TjMaxJumpTime { get { return tjMaxJumpTime; } }
    public float TjMinJumpTime { get { return tjMinJumpTime; } }
    public float TjMaxDissonance { get { return tjMaxDissonance; } }

    public float HorizBoostMinGravity { get { return horizBoostMinGravity; } }
    public float HorizBoostMaxGravity { get { return horizBoostMaxGravity; } }
    public float HorizBoostChargeGravity { get { return horizBoostChargeGravity; } }
    public float HorizBoostEndGravity { get { return horizBoostEndGravity; } }
    public float HorizBoostMaxChargeTime { get { return horizBoostMaxChargeTime; } }
    public float HorizBoostChargeRotationSpeed { get { return horizBoostChargeRotationSpeed; } }
    public float HorizBoostMaxTime { get { return horizBoostMaxTime; } }
    public float HorizBoostRotation { get { return horizBoostRotation; } }
    public float HorizBoostAirReverseGravity { get { return horizBoostAirReverseGravity; } }
    public float HorizBoostNonAirReverseGravity { get { return horizBoostNonAirReverseGravity; } }
    public float HorizBoostMinActivationX { get { return horizBoostMinActivationX; } }
    public float HorizBoostMaxActivationX { get { return horizBoostMaxActivationX; } }
    public float VertBoostMinVel { get { return vertBoostMinVel; } }
    public float VertBoostMaxVel { get { return vertBoostMaxVel; } }
    public float VertBoostGravity { get { return vertBoostGravity; } }
    public float VertBoostMaxChargeTime { get { return vertBoostMaxChargeTime; } }
    public float VertBoostMaxTime { get { return vertBoostMaxTime; } }
    public float VertBoostRotationSpeed { get { return vertBoostRotationSpeed; } }
    public float VertBoostMaxSpeedX { get { return vertBoostMaxSpeedX; } }

    public float GpSuspensionTime { get { return gpSuspensionTime; } }
    public float GpDownSpeed { get { return gpDownSpeed; } }

    public float DiveInitVel { get { return diveInitVel; } }
    public float DiveGravity { get { return diveGravity; } }
    public float DiveRecoveryTime { get { return diveRecoveryTime;  } }

    public float DefaultGravity { get { return defaultGravity; } }
    public float StickToGroundMultiplier { get { return maxSpeed; } }

    public float DissonanceForAirReverse { get { return dissonanceForAirReverse; } }
    public float AirReverseMinActivationSpeed { get { return airReverseMinActivationSpeed; } }

    public float InstantRotationSpeed { get { return instantRotationSpeed; } }
    public float GroundRotationSpeed { get { return groundRotationSpeed; } }
    public float AirRotationSpeed { get { return airRotationSpeed; } }
    public float BoostAftermathRotationSpeed { get { return boostAftermathRotationSpeed; } }
    public float DiveRotationSpeed { get { return diveRotationSpeed; } }
    public float GroundBoostRotationSpeed { get { return groundBoostRotationSpeed; } }

    public float SlideForce { get { return slideForce; } }
    public float SlideMaxSpeed { get { return slideMaxSpeed; } }
    public float SlideRecoveryPace { get { return slideRecoveryPace; } }

    public float GlideRotationSpeed { get { return glideRotationSpeed; } }
    public float GlideRotationSpeedSensitivity { get { return glideRotationSpeedSensitivity; } }
    public float GlideRotationSpeedGravity { get { return glideRotationSpeedGravity; } }
    public float GlideTiltSensitivity { get { return glideTiltSensitivity; } }
    public float GlideAirLoss { get { return glideAirLoss; } }
    public float GlideHorizontalSpeed { get { return glideHorizontalSpeed; } }
    public float GlideVerticalSpeed { get { return glideVerticalSpeed; } }
    public float GlideMaxHorizontalSpeed { get { return glideMaxHorizontalSpeed; } }
    public float GlideMaxVerticalSpeed { get { return glideMaxVerticalSpeed; } }
    public float GlideEnterDriftSpeed { get { return glideEnterDriftSpeed; } }
    public float GlideTimeToEnterDrift { get { return glideTimeToStartDrift; } }
    public float GlideDriftDuration { get { return glideDriftDuration; } }
    public Vector2 GlideDriftMovementVector {  get { return glideDriftMovementVector; } }
    public float GlideDriftBufferDuration { get { return glideDriftBufferDuration; } }
    public float GlideWeight { get { return glideWeight; } }
    public float GlideMaxTilt { get { return glideMaxTilt; } }
    public float GlideMaxRoll { get { return glideMaxRoll;  } }
    public float GlideGravity { get { return glideGravity; } }
    public float GlideCameraAdjustRatio { get { return glideCameraAdjustRatio; } }
    public float GlideNonControlTime { get { return glideNonControlTime; } }
    public float GlideMinCamAngleX { get { return glideMinCamAngleX; } }
    public float GlideMaxCamAngleX { get { return glideMaxCamAngleX; } }
    public float GlideJumpSpeed { get { return glideJumpSpeed; } }
    public float GlideJumpTime { get { return glideJumpTime; } }
    public float GlideXSensitivity { get { return glideXSensitivity; } }
    public float GlideXGravity { get { return glideXGravity; } }


    static MovementSettingsSO _instance;
    public static MovementSettingsSO Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.Load<MovementSettingsSO>("MovementSettings");
            }
            return _instance;
        }
    }
}
