using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] MoveExecuter me;
    //[SerializeField] MovementMaster mm;
    [SerializeField] Animator animator;
    bool onGround = false;
    bool diving, startRunning;
    float acceleration = 0f;
    float lastSpeed = 0f;
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

        animator.SetBool(s, true);
    }
    void Start()
    {
        //initialize bools
        diving = false;
        startRunning = false;

        //Initialize all Animation variables as false
        animator.SetBool("IDLE", false);
        animator.SetBool("FALLING", false);
        animator.SetBool("WALKING", false);
        animator.SetBool("RUNNING", false);
        animator.SetBool("DIVE", false);
        animator.SetBool("DIVELANDING", false);
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
    }

    void FixedUpdate()
    {

        float speed = me.GetCurrentMove().GetHorizSpeedThisFrame().magnitude;
        StartCoroutine(GetAcceleration(speed));
        temp = me.GetCurrentMove().AsString();

        //move changed
        if (temp != cM)
        {
            lM = cM;
            cM = temp;
        }
        //Debug.Log(cM);

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
            if (lM == "dive" && diving)
            {
                diving = false;
                currentMove("DIVERECOVERY");
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
            onGround = true;
            if(lM == "idle" && startRunning)
            {
                currentMove("STARTRUN");
                startRunning = false;
            }
            else if (lM == "dive" && diving)
            {
                diving = false;
                currentMove("DIVERECOVERY");
            }
            else if(speed > 6.25f && !diving)
            {
                currentMove("JETPACKRUN");
            }
            else if (speed < 3.15f && !diving)
            {
                if (Mathf.Abs(acceleration) <= 5f)
                    currentMove("WALKING");
                else
                    currentMove("RUNNING");
            }
            else
            {
                currentMove("RUNNING");

            }
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

        }
        else if (cM == "doublejump")
        {
            onGround = false;
            currentMove("SECONDJUMP");

        }
        else if (cM == "triplejump")
        {
            onGround = false;
            currentMove("THIRDJUMP");

        }
        else if (cM == "horizairboostcharge")
        {
            onGround = false;
            currentMove("HCHARGE");

        }
        else if (cM == "horizairboost")
        {
            onGround = false;
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

            onGround = true;
            currentMove("HARDTURN");
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
}
