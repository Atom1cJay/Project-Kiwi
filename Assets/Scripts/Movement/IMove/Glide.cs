using System;
using System.Collections;
using UnityEngine;

public class Glide : AMove
{
    bool groundPoundPending; // Temp
    bool glidePending; // Temp
    bool drifting;
    bool canDriftAgain;
    float driftAgainTime;
    float vertVel;
    float horizVel;
    float tilt; // Angle forward/backward. Negative = forward
    float enterDriftSpeed;
    float rotSpeed;
    float endDrift;

    public Glide(MovementInputInfo mii, MovementInfo mi, MovementSettingsSO ms, float horizVel, float vertVel) : base(ms, mi, mii)
    {
        this.horizVel = horizVel;

        drifting = false;
        endDrift = 0f;
        canDriftAgain = true;
        driftAgainTime = Time.time + 1f;

        enterDriftSpeed = movementSettings.GlideEnterDriftSpeed;
        mii.OnGroundPound.AddListener(() => groundPoundPending = true);
        mii.OnGlide.AddListener(() => glidePending = true);
    }

    public override void AdvanceTime()
    {
        //Debug.Log("Left: " + mii.GetHorizontalInput() + ", " + "Right: " + mii.GetCameraInput());
        // Tilt

        //set input to val
        float val = mii.GetHorizontalInput().y;

        //check for drifting
        drifting = Time.time < endDrift;
        canDriftAgain = Time.time >= driftAgainTime;
        //clamp to 0
        if (horizVel < enterDriftSpeed && canDriftAgain && !drifting)
        {
            MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", (StillInDriftSpeed(horizVel)));
        }

        float x, y;

        if (drifting)
        {
            //if drifting force vertical speed


            x = (Mathf.Pow(val, 3)) * movementSettings.GlideHorizontalSpeed;
            y = -(Mathf.Pow(val, 3)) * movementSettings.GlideVerticalSpeed;

            horizVel += x * Time.deltaTime;
            vertVel += (y - movementSettings.GlideAirLoss) * Time.deltaTime;


            //drifting clamping
            if (horizVel < movementSettings.GlideDriftMovementVector.x)
            {
                horizVel = movementSettings.GlideDriftMovementVector.x;
            }
            else if (horizVel > movementSettings.GlideMaxHorizontalSpeed)
            {
                horizVel = movementSettings.GlideMaxHorizontalSpeed;
            }
            if (vertVel > movementSettings.GlideDriftMovementVector.y)
                vertVel += (movementSettings.GlideDriftMovementVector.y - vertVel) * 0.1f;

            Debug.Log("hor: " + horizVel + ",   " + "ver: " + vertVel);

        }
        else
        {

            x = (Mathf.Pow(val, 3)) * movementSettings.GlideHorizontalSpeed;
            y = -(Mathf.Pow(val, 3)) * movementSettings.GlideVerticalSpeed;

            if (val > -.1f && val < .1f)
                x = 0.5f;
            horizVel += x * Time.deltaTime;
            vertVel += (y - movementSettings.GlideAirLoss) * Time.deltaTime;

            //clamping
            if (horizVel < 2.5f)
                horizVel = 2.5f;
            else if (horizVel > movementSettings.GlideMaxHorizontalSpeed)
            {
                horizVel = movementSettings.GlideMaxHorizontalSpeed;
            }
            if (vertVel > movementSettings.GlideMaxVerticalSpeed)
                vertVel = movementSettings.GlideMaxVerticalSpeed;
            Debug.Log("hor: " + horizVel + ",   " + "ver: " + vertVel);
        }

        //// tilt += movementSettings.GlideTiltSensitivity * mii.GetHorizontalInput().y * Time.deltaTime;
        
        //horizVel += movementSettings.GliderWeight * Mathf.Sin(tilt);
        //vertVel = -horizVel * Mathf.Tan(tilt);

        // Rotation
        rotSpeed += mii.GetHorizontalInput().x * movementSettings.GlideRotationSpeedSensitivity * Time.deltaTime;
        if (rotSpeed > movementSettings.GlideRotationSpeed)
        {
            rotSpeed = movementSettings.GlideRotationSpeed;
        }
        if (rotSpeed < -movementSettings.GlideRotationSpeed)
        {
            rotSpeed = -movementSettings.GlideRotationSpeed;
        }
    }

    private void Invoke(string v1, float v2)
    {
        throw new NotImplementedException();
    }

    public override Vector2 GetHorizSpeedThisFrame()
    {
        return ForwardMovement(horizVel);
    }

    public override float GetVertSpeedThisFrame()
    {
        return vertVel;
    }

    public override float GetRotationSpeed()
    {
        return rotSpeed;
    }

    public override IMove GetNextMove()
    {
        if (groundPoundPending)
        {
            return new GroundPound(mii, mi, movementSettings);
        }
        else if (mi.TouchingGround())
        {
            if (mii.GetHorizontalInput().magnitude <= .25f)
            {
                return new Idle(mii, mi, movementSettings);
            }
            else
            {
                return new Run(mii, mi, movementSettings, ForwardMovement(horizVel));
            }
        }
        else if (glidePending)
        {
            return new Fall(mii, mi, movementSettings, ForwardMovement(horizVel), false);
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
    IEnumerator StillInDriftSpeed(float pastSpeed)
    {
        yield return new WaitForSeconds(movementSettings.GlideTimeToEnterDrift);
        if (!drifting)
        {
            if (pastSpeed < enterDriftSpeed && horizVel < enterDriftSpeed)
            {
                Debug.Log("start driftin bitch");
                drifting = true;
                endDrift = Time.time + movementSettings.GlideDriftDuration;
                driftAgainTime = endDrift + movementSettings.GlideDriftBufferDuration;
            }
        }
    }

}
