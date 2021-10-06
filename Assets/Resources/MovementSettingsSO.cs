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
    [Header("Run Settings")]
    [SerializeField] float maxSpeed;
    [SerializeField] float maxSpeedAbsolute;
    [SerializeField] float runSensitivityX;
    [SerializeField] float runGravityX;
    [SerializeField] float runGravityXOverTopSpeed;
    [SerializeField] float runGravityXOverTopSpeedNoInput;

    [Header("Air Settings")]
    [SerializeField] float airSensitivityX;
    [SerializeField] float airGravityX;

    [Header("Hard Turn Settings")]
    [SerializeField] float hardTurnGravityX;

    [Header("Dive Settings")]
    [SerializeField] float diveSpeedX;

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
    [SerializeField] float jumpSensitivityX;
    [SerializeField] float jumpAdjustSensitivityX;
    [SerializeField] float jumpGravityX;
    [SerializeField] float jumpSensitivityReverseX;
    [SerializeField] float jumpSpeedDecRateOverMaxSpeed;

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
    [SerializeField] float tjAirSensitivityX;
    [SerializeField] float tjInputGravityX;
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

    [Header("Horiz Boost Settings")]
    [SerializeField] float horizBoostMinGravity;
    [SerializeField] float horizBoostMaxGravity;
    [SerializeField] float horizBoostGravityIncRate;
    [SerializeField] float horizBoostChargeVertNeutralizeTime;
    [SerializeField] float horizBoostChargeRotationSpeed;
    [SerializeField] float horizBoostEndGravity;
    [SerializeField] float horizBoostMaxChargeTime;
    [SerializeField] float horizBoostMaxTime;
    [SerializeField] float horizBoostRotation;
    [SerializeField] float horizBoostAirReverseGravity;
    [SerializeField] float horizBoostNonAirReverseGravity;
    [SerializeField] float horizBoostMinActivationBoostX;
    [SerializeField] float horizBoostMaxActivationBoostX;
    [SerializeField] float horizBoostMinActivationBoostGroundX;
    [SerializeField] float horizBoostMaxActivationBoostGroundX;
    [SerializeField] float horizBoostSpeedX;
    [SerializeField] float horizBoostChargeGravityX;
    [SerializeField] float horizBoostToGroundBoostSensitivity;

    [Header("Vertical Boost Settings")]
    [SerializeField] float vertBoostChargeGravityX;
    [SerializeField] float vertBoostMinVel;
    [SerializeField] float vertBoostMaxVel;
    [SerializeField] float vertBoostChargeGravity;
    [SerializeField] float vertBoostGravity;
    [SerializeField] float vertBoostMaxChargeTime;
    [SerializeField] float vertBoostMaxTime;
    [SerializeField] float vertBoostRotationSpeed;
    [SerializeField] float vertBoostMaxSpeedX;

    [Header("Push Boost Settings")]
    [SerializeField] float pushSpeedDecRate;
    [SerializeField] float pushIdealResetRate;
    [SerializeField] float pushMaxHoldTime;
    [SerializeField] float pushSpeedIncMin;
    [SerializeField] float pushSpeedIncMax;
    [SerializeField] float pushSpeedMaintainTime;

    [Header("Ground Boost Settings")]
    [SerializeField] float groundBoostMaxSpeedX;
    [SerializeField] float groundBoostSensitivityX;
    [SerializeField] float groundBoostGravityX;

    [Header("Ground Pound Settings")]
    [SerializeField] float gpSuspensionTime;
    [SerializeField] float gpDownSpeed;
    [SerializeField] float gpLandTime;

    [Header("Dive Settings")]
    [SerializeField] float diveInitVel;
    [SerializeField] float diveGravity;
    [SerializeField] float diveRecoveryTime;

    [Header("Misc. Vertical")]
    [SerializeField] float defaultGravity;
    [SerializeField] float stickToGroundMultiplier;

    [Header("Air Reverse Settings")]
    [SerializeField] float dissonanceForAirReverse;
    [SerializeField] float airReverseMinActivationSpeed;

    [Header("Rotation Settings")]
    [SerializeField] float instantRotationSpeed;
    [SerializeField] float groundRotationSpeed;
    [SerializeField] float groundRotationSpeedMaxXSpeed;
    [SerializeField] float airRotationSpeed;
    [SerializeField] float boostAftermathRotationSpeed;
    [SerializeField] float diveRotationSpeed;
    [SerializeField] float groundBoostRotationSpeed;

    [Header("Slide Settings")]
    [SerializeField] float slideForceToZero;
    [SerializeField] float slideForceNegative;
    [SerializeField] float slideMaxSpeed;
    [SerializeField] float slideRecoveryPace;

    [Header("Glide Settings")]
    [SerializeField] float glideRotationSpeed;
    [SerializeField] float glideAirLoss;
    [SerializeField] float glideMaxHorizontalSpeed;
    [SerializeField] float glideMaxHorizontalSpeedBoosted;
    [SerializeField] float glideNonControlTime;
    [SerializeField] float glideJumpSpeed;
    [SerializeField] float glideJumpTime;
    [SerializeField] float glideXSensitivity;
    [SerializeField] float glideXGravity;
    [SerializeField] float glideXSensitivityBoosting;
    [SerializeField] float glideXGravityOutOfBoost;

    [Header("Swim Settings")]
    [SerializeField] float swimRotationSpeed;
    [SerializeField] float swimMaxSpeedNormal;
    [SerializeField] float swimMaxSpeedBoosted;
    [SerializeField] float swimSensitivityNormal;
    [SerializeField] float swimSensitivityBoosted;
    [SerializeField] float swimGravity;
    [SerializeField] float swimOutOfBoostGravity;

    [Header("Attack Settings")]
    [SerializeField] Attack diveAttack;
    [SerializeField] Attack jumpAttack;
    [SerializeField] Attack horizBoostAttack;
    [SerializeField] Attack vertBoostAttack;
    [SerializeField] Attack groundPoundAttack;

    [Header("Knockback Settings")]
    [SerializeField] float knockbackYAngle; // Degrees (90 = max)
    [SerializeField] float knockbackYGravity;
    [SerializeField] float knockbackRecoveryTime;

    /// public, readonly variables
    public float MaxSpeed { get { return maxSpeed; } }
    public float MaxSpeedAbsolute { get { return maxSpeedAbsolute; } }
    public float RunSensitivityX { get { return runSensitivityX; } }
    public float RunGravityX { get { return runGravityX; } }
    public float RunGravityXOverTopSpeed { get { return runGravityXOverTopSpeed; } }
    public float RunGravityXOverTopSpeedNoInput { get { return runGravityXOverTopSpeedNoInput; } }

    public float AirSensitivityX { get { return airSensitivityX; } }
    public float AirGravityX { get { return airGravityX; } }

    public float HardTurnGravityX { get { return hardTurnGravityX; } }

    public float DiveSpeedX { get { return diveSpeedX; } }

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
    public float JumpSensitivityX { get { return jumpSensitivityX; } }
    public float JumpAdjustSensitivityX { get { return jumpAdjustSensitivityX; } }
    public float JumpSensitivityReverseX { get { return jumpSensitivityReverseX; } }
    public float JumpGravityX { get { return jumpGravityX; } }
    public float JumpSpeedDecRateOverMaxSpeed { get { return jumpSpeedDecRateOverMaxSpeed; } }

    public float ReverseCoyoteTime { get { return reverseCoyoteTime; } }
    public float CoyoteTime { get { return coyoteTime; } }

    public float DissonanceForHardTurn { get { return dissonanceForHardTurn; } }
    public float HardTurnMinSpeed { get { return hardTurnMinSpeed; } }
    public float HardTurnTime { get { return hardTurnTime; } }

    public float TjAirSensitivityX { get { return tjAirSensitivityX; } }
    public float TjInputGravityX { get { return tjInputGravityX; } }
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
    public float HorizBoostGravityIncRate { get { return horizBoostGravityIncRate; } }
    public float HorizBoostChargeVertNeutralizeTime { get { return horizBoostChargeVertNeutralizeTime; } }
    public float HorizBoostEndGravity { get { return horizBoostEndGravity; } }
    public float HorizBoostMaxChargeTime { get { return horizBoostMaxChargeTime; } }
    public float HorizBoostChargeRotationSpeed { get { return horizBoostChargeRotationSpeed; } }
    public float HorizBoostMaxTime { get { return horizBoostMaxTime; } }
    public float HorizBoostRotation { get { return horizBoostRotation; } }
    public float HorizBoostAirReverseGravity { get { return horizBoostAirReverseGravity; } }
    public float HorizBoostNonAirReverseGravity { get { return horizBoostNonAirReverseGravity; } }
    public float HorizBoostMinActivationBoostX { get { return horizBoostMinActivationBoostX; } }
    public float HorizBoostMaxActivationBoostX { get { return horizBoostMaxActivationBoostX; } }
    public float HorizBoostMinActivationBoostGroundX { get { return horizBoostMinActivationBoostGroundX; } }
    public float HorizBoostMaxActivationBoostGroundX { get { return horizBoostMaxActivationBoostGroundX; } }
    public float HorizBoostSpeedX { get { return horizBoostSpeedX; } }
    public float HorizBoostChargeGravityX { get { return horizBoostChargeGravityX; } }
    public float HorizBoostToGroundBoostSensitivity { get { return horizBoostToGroundBoostSensitivity; } }

    public float VertBoostMinVel { get { return vertBoostMinVel; } }
    public float VertBoostMaxVel { get { return vertBoostMaxVel; } }
    public float VertBoostGravity { get { return vertBoostGravity; } }
    public float VertBoostChargeGravity { get { return vertBoostChargeGravity; } }
    public float VertBoostMaxChargeTime { get { return vertBoostMaxChargeTime; } }
    public float VertBoostMaxTime { get { return vertBoostMaxTime; } }
    public float VertBoostRotationSpeed { get { return vertBoostRotationSpeed; } }
    public float VertBoostMaxSpeedX { get { return vertBoostMaxSpeedX; } }
    public float VertBoostChargeGravityX { get { return vertBoostChargeGravityX; } }

    public float PushSpeedMaintainTime { get { return pushSpeedMaintainTime; } }

    public float GroundBoostMaxSpeedX { get { return groundBoostMaxSpeedX; } }
    public float GroundBoostSensitivityX { get { return groundBoostSensitivityX; } }
    public float GroundBoostGravityX { get { return groundBoostGravityX; } }

    public float GpSuspensionTime { get { return gpSuspensionTime; } }
    public float GpDownSpeed { get { return gpDownSpeed; } }
    public float GpLandTime { get { return gpLandTime; } }

    public float DiveInitVel { get { return diveInitVel; } }
    public float DiveGravity { get { return diveGravity; } }
    public float DiveRecoveryTime { get { return diveRecoveryTime;  } }

    public float DefaultGravity { get { return defaultGravity; } }
    public float StickToGroundMultiplier { get { return maxSpeed; } }

    public float DissonanceForAirReverse { get { return dissonanceForAirReverse; } }
    public float AirReverseMinActivationSpeed { get { return airReverseMinActivationSpeed; } }

    public float InstantRotationSpeed { get { return instantRotationSpeed; } }
    public float GroundRotationSpeed { get { return groundRotationSpeed; } }
    public float GroundRotationSpeedMaxXSpeed { get { return groundRotationSpeedMaxXSpeed; } }
    public float AirRotationSpeed { get { return airRotationSpeed; } }
    public float BoostAftermathRotationSpeed { get { return boostAftermathRotationSpeed; } }
    public float DiveRotationSpeed { get { return diveRotationSpeed; } }
    public float GroundBoostRotationSpeed { get { return groundBoostRotationSpeed; } }

    public float SlideForceToZero { get { return slideForceToZero; } }
    public float SlideForceNegative { get { return slideForceNegative; } }
    public float SlideMaxSpeed { get { return slideMaxSpeed; } }
    public float SlideRecoveryPace { get { return slideRecoveryPace; } }

    public float GlideRotationSpeed { get { return glideRotationSpeed; } }
    public float GlideAirLoss { get { return glideAirLoss; } }
    public float GlideMaxHorizontalSpeed { get { return glideMaxHorizontalSpeed; } }
    public float GlideMaxHorizontalSpeedBoosted { get { return glideMaxHorizontalSpeedBoosted; } }
    public float GlideNonControlTime { get { return glideNonControlTime; } }
    public float GlideJumpSpeed { get { return glideJumpSpeed; } }
    public float GlideJumpTime { get { return glideJumpTime; } }
    public float GlideXSensitivity { get { return glideXSensitivity; } }
    public float GlideXGravity { get { return glideXGravity; } }
    public float GlideXSensitivityBoosting { get { return glideXSensitivityBoosting; } }
    public float GlideXGravityOutOfBoost { get { return glideXGravityOutOfBoost; } }

    public float SwimRotationSpeed { get { return swimRotationSpeed; } }
    public float SwimMaxSpeedNormal { get { return swimMaxSpeedNormal; } }
    public float SwimMaxSpeedBoosted { get { return swimMaxSpeedBoosted; } }
    public float SwimSensitivityNormal { get { return swimSensitivityNormal; } }
    public float SwimSensitivityBoosted { get { return swimSensitivityBoosted; } }
    public float SwimGravity { get { return swimGravity; } }
    public float SwimOutOfBoostGravity { get { return swimOutOfBoostGravity; } }

    public Attack DiveAttack { get { return diveAttack; } }
    public Attack JumpAttack { get { return jumpAttack; } }
    public Attack HorizBoostAttack { get { return horizBoostAttack; } }
    public Attack VertBoostAttack { get { return vertBoostAttack; } }
    public Attack GroundPoundAttack { get { return groundPoundAttack; } }

    public float KnockbackYAngle { get { return knockbackYAngle; } } // Degrees (90 = max)
    public float KnockbackYGravity { get { return knockbackYGravity; } }
    public float KnockbackRecoveryTime { get { return knockbackRecoveryTime; } }

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
