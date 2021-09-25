using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] MoveExecuter me;
    //[SerializeField] MovementMaster mm;
    [SerializeField] Animator animator;
    bool onGround = false;
    bool diving, startRunning, canTransitionToIdle, stopping;
    int transitioningIdle = 0;
    float acceleration = 0f;
    float lastSpeed = 0f;
    float extraStopTime = 0f;
    bool resetSlowDown = true;
    float vertSpeed, jetrunThreshold, fallThreshold;
    string lM, temp, cM;
    //Sets all bools to false except on ground
    void currentMove(string s)
    {

        animator.SetBool("IDLE", false);
        animator.SetBool("FALLING", false);
        animator.SetBool("RUNNING", false);
        animator.SetBool("DIVE", false);
        animator.SetBool("FIRSTJUMP", false);
        animator.SetBool("SECONDJUMP", false);
        animator.SetBool("THIRDJUMP", false);
        animator.SetBool("HCHARGE", false);
        animator.SetBool("HBOOST", false);
        animator.SetBool("VCHARGE", false);
        animator.SetBool("VBOOST", false);
        animator.SetBool("ONGROUND", onGround);
        animator.SetBool("DIVERECOVERY", false);
        animator.SetBool("GROUNDPOUND", false);
        animator.SetBool("SKID", false);
        animator.SetBool("HARDTURN", false);
        animator.SetBool("JETPACKRUN", false);
        animator.SetBool("WALKING", false);
        animator.SetBool("STARTRUN", false);
        animator.SetBool("SWIM", false);
        animator.SetBool("STOPPING", false);

        animator.SetBool(s, true);
    }
    void Start()
    {
        //initialize vars
        diving = false;
        canTransitionToIdle = true;
        stopping = false;
        startRunning = false;
        jetrunThreshold = MovementSettingsSO.Instance.MaxSpeed;
        fallThreshold = (MovementSettingsSO.Instance.JumpInitVel - MovementSettingsSO.Instance.DefaultGravity * 0.3f);

        //Initialize all Animation variables as false
        animator.SetBool("IDLE", false);
        animator.SetBool("FALLING", false);
        animator.SetBool("WALKING", false);
        animator.SetBool("RUNNING", false);
        animator.SetBool("DIVE", false);
        animator.SetBool("FIRSTJUMP", false);
        animator.SetBool("SECONDJUMP", false);
        animator.SetBool("THIRDJUMP", false);
        animator.SetBool("HCHARGE", false);
        animator.SetBool("HBOOST", false);
        animator.SetBool("VCHARGE", false);
        animator.SetBool("VBOOST", false);
        animator.SetBool("ONGROUND", onGround);
        animator.SetBool("DIVERECOVERY", false);
        animator.SetBool("GROUNDPOUND", false);
        animator.SetBool("SKID", false);
        animator.SetBool("HARDTURN", false);
        animator.SetBool("JETPACKRUN", false);
        animator.SetBool("STARTRUN", false);
        animator.SetBool("SWIM", false);
        animator.SetBool("STOPPING", false);


        animator.SetBool("CANTRANSITIONTOIDLE", canTransitionToIdle);
    }

    void FixedUpdate()
    {
        animator.SetBool("CANTRANSITIONTOIDLE", canTransitionToIdle);

        float speed = me.GetCurrentMove().GetHorizSpeedThisFrame().magnitude;
        StartCoroutine(GetAcceleration(speed));
        temp = me.GetCurrentMove().AsString();
        vertSpeed = me.GetCurrentMove().GetVertSpeedThisFrame();
        //Debug.Log("threshold " + fallThreshold + " sspeed:" + vertSpeed);



        //move changed
        if (temp != cM)
        {
            lM = cM;
            cM = temp;
        }

        //Debug.Log(cM);
        //walking state not implemented yet

        if (cM == "walking")
        {
            onGround = true;
            currentMove("WALKING");
        }
        else if (cM == "idle")
        {
            startRunning = true;
            onGround = true;
            resetSlowDown = true;
            if (lM == "dive" && diving)
            {
                diving = false;
                currentMove("DIVERECOVERY");
            }
            else if (stopping)
            {
                StartCoroutine(FinishedStopping(extraStopTime));
                currentMove("STOPPING");
            }
            else
                currentMove("IDLE");
        }
        else if (cM == "fall")
        {
            onGround = false;
            currentMove("FALLING");
        }
        else if (cM == "run")
        {

            //Debug.Log("speed:  " + speed + "bool" + animator.GetBool("STOPPING"));
            onGround = true;
            if (lM == "dive" && diving)
            {
                diving = false;
                currentMove("DIVERECOVERY");
                StartCoroutine(CanTransitionToIdleAgain(1f));
            }
            else if (acceleration <= -15f && speed >= jetrunThreshold * 0.75f)
            {
                //Slow stop
                if(speed > jetrunThreshold)
                {
                    animator.SetFloat("SLOWDOWNTIME", 0.8f);
                    extraStopTime = 1f;
                }
                else
                {
                    animator.SetFloat("SLOWDOWNTIME", 0.6f);
                    extraStopTime = .05f;
                }
                currentMove("STOPPING");
                stopping = true;
            }
            else if ((lM == "idle" || lM == "hardturn") && startRunning)
            {

                StartCoroutine(CanTransitionToIdleAgain(1f));
                currentMove("STARTRUN");
                startRunning = false;
            }
            else if (speed > jetrunThreshold && !diving)
            {
                if(acceleration >= -7.5f)
                    currentMove("JETPACKRUN");
            }
            else if (speed < 4f && !diving && !stopping)
            {
                if (Mathf.Abs(acceleration) <= 3f)
                    currentMove("WALKING");
                else
                    currentMove("RUNNING");
            }
            else
            {
                if (acceleration >= -7.5f)
                    currentMove("RUNNING");
            }
            diving = false;
        }
        else if (cM == "dive")
        {
            onGround = false;
            diving = true;
            currentMove("DIVE");

        }
        else if (cM == "jump")
        {
            onGround = false;
            currentMove("FIRSTJUMP");
            if (vertSpeed <= fallThreshold)
                currentMove("FALLING");

        }
        else if (cM == "doublejump")
        {
            onGround = false;
            currentMove("FIRSTJUMP"); // Temp, while animations are same

            if (vertSpeed <= fallThreshold)
                currentMove("FALLING");

        }
        else if (cM == "triplejump")
        {
            onGround = false;
            currentMove("THIRDJUMP");

            if (vertSpeed <= fallThreshold)
                currentMove("FALLING");

        }
        else if (cM == "horizairboostcharge")
        {
            onGround = false;
            currentMove("HCHARGE");

        }
        else if (cM == "horizairboost")
        {
            onGround = false;
            StartCoroutine(CanTransitionToIdleAgain(1f));
            currentMove("HBOOST");

        }
        else if (cM == "vertairboostcharge")
        {
            onGround = false;
            currentMove("VCHARGE");

        }
        else if (cM == "vertairboost")
        {
            onGround = false;
            currentMove("VBOOST");

        }
        else if (cM == "idle")
        {

            onGround = true;
            currentMove("IDLE");
        }
        else if (cM == "hardturn")
        {

            startRunning = true;
            onGround = true;
            currentMove("HARDTURN");
            StartCoroutine(CanTransitionToIdleAgain(1f));
        }

        else if (cM == "groundpound")
        {

            onGround = false;
            currentMove("GROUNDPOUND");
        }
        else if (cM == "slide")
        {

            onGround = true;
            currentMove("SKID");
        }
        else if (cM == "sliderecovery")
        {

            onGround = true;
            currentMove("SKID");
        }
        else if (cM == "swim")
        {

            onGround = true;
            currentMove("SWIM");
        }
        else
        {

            onGround = true;
            currentMove("IDLE");
        }
    }

    IEnumerator GetAcceleration(float t)
    {
        yield return new WaitForSeconds(0.1f);
        acceleration = (me.GetCurrentMove().GetHorizSpeedThisFrame().magnitude - t) / 0.1f;
    }

    IEnumerator FinishedStopping(float t)
    {
        yield return new WaitForSeconds(t);
        stopping = false;
    }

    IEnumerator CanTransitionToIdleAgain(float t)
    {
        transitioningIdle++;
        
        canTransitionToIdle = false;
        yield return new WaitForSeconds(t);
        if(transitioningIdle == 1)
            canTransitionToIdle = true;

        transitioningIdle--;


    }
}
