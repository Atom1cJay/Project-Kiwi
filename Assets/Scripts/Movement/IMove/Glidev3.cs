using System;
using System.Collections;
using UnityEngine;

public class Glidev3 : AMove
{
    float vertVel;
    Vector2 horizVector;
    bool objectHitPending;
    bool glideReleasePending;
    bool groundPoundPending;

    public Glidev3(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, Vector2 horizVector) : base(ms, mi, mii)
    {
        this.horizVector = horizVector;
        if (this.horizVector.magnitude > movementSettings.GlideMaxHorizontalSpeed)
        {
            this.horizVector = this.horizVector.normalized * movementSettings.GlideMaxHorizontalSpeed;
        }
        MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", GiveControl());
        mii.OnGlide.AddListener(() => glideReleasePending = true);
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mi.OnCharContTouchSomething.AddListener(() => objectHitPending = true);
    }

    public override void AdvanceTime()
    {
        horizVector += mii.GetRelativeHorizontalInputToCamera() * movementSettings.GlideXSensitivity * Time.deltaTime;
        if (horizVector.magnitude > movementSettings.GlideMaxHorizontalSpeed)
        {
            horizVector = horizVector.normalized * movementSettings.GlideMaxHorizontalSpeed;
        }
        if (mii.GetRelativeHorizontalInputToCamera().magnitude < 0.1f)
        {
            float magn = horizVector.magnitude;
            magn -= movementSettings.GlideXGravity * Time.deltaTime;
            horizVector = horizVector.normalized * magn;
        }
    }

    IEnumerator GiveControl()
    {
        float timePassed = 0;
        float tilt;

        while (timePassed < movementSettings.GlideJumpTime)
        {
            tilt = (timePassed / movementSettings.GlideJumpTime) * Mathf.PI / 2;
            vertVel = -Mathf.Cos(tilt) * movementSettings.GlideJumpSpeed - movementSettings.GlideAirLoss;
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (timePassed < movementSettings.GlideNonControlTime)
        {
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        vertVel = -movementSettings.GlideAirLoss;
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return horizVector;
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override float GetRotationSpeed()
    {
        return movementSettings.GlideRotationSpeed;
    }

    public override IMove GetNextMove()
    {
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        if (glideReleasePending)
        {
            return new Fall(mii, mi, movementSettings, horizVector, false);
        }
        if (mi.TouchingGround())
        {
            if (mii.GetHorizontalInput().magnitude <= .25f)
            {
                return new Idle(mii, mi, movementSettings);
            }
            else
            {
                return new Run(mii, mi, movementSettings, horizVector);
            }
        }
        else if (objectHitPending)
        {
            return new Fall(mii, mi, movementSettings, horizVector, false);
        }
        return this;
    }

    public override bool IncrementsTJcounter()
    {
        return false;
    }

    public override bool TJshouldBreak()
    {
        return true;
    }

    public override bool AdjustToSlope()
    {
        return false;
    }

    public override string AsString()
    {
        return "glide";
    }
}
