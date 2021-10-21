using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandlerV2 : MonoBehaviour
{
    [SerializeField] MoveExecuter me;
    //[SerializeField] MovementMaster mm;
    [SerializeField] Animator animator;
    [SerializeField] List<string[]> pairs = new List<string[]>();
    bool onGround = false;
    bool diving, startRunning, stopping;
    int movesExtending = 0;
    int anyStateExtending = 0;
    float acceleration = 0f;
    float lastSpeed = 0f;
    float extraStopTime = 0f;
    bool resetSlowDown = true;
    bool extending, anyStateTransition;
    float vertSpeed, jetrunThreshold, fallThreshold;
    string lM, temp, cM;

    string extendedMove = "";
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
        animator.SetBool("BOOSTSLIDE", false);

        animator.SetBool(s, true);
    }
    void Start()
    {
        //initialize vars
        diving = false;
        stopping = false;
        extending = false;
        startRunning = false;
        jetrunThreshold = MovementSettingsSO.Instance.MaxSpeed + 1f;
        fallThreshold = (MovementSettingsSO.Instance.JumpInitVel - MovementSettingsSO.Instance.DefaultGravity * 0.2f);
        anyStateTransition = true;

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
        animator.SetBool("BOOSTSLIDE", false);



    }

    void FixedUpdate()
    {

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
        if (extending)
        {
            currentMove(extendedMove);
        }
        else if (cM == "walking")
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
                anyStateTransition = false;
                StartCoroutine(AnyStateAgain(0.5f));
                diving = false;
            }
            else if (stopping)
            {
                stopping = false;
                StartCoroutine(ExtendMove("STOPPING", 0.25f));
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
                StartCoroutine(ExtendMove("DIVERECOVERY", 0.35f));
            }
            else if (acceleration <= -15f && speed >= jetrunThreshold * 0.6f && !diving)
            {
                //Slow stop
                if (speed > jetrunThreshold)
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
                
                currentMove("STARTRUN");
                anyStateTransition = false;
                StartCoroutine(AnyStateAgain(0.5f));
                StartCoroutine(ExtendMove("STARTRUN", 0.4f));
                startRunning = false;
            }
            else if (speed > jetrunThreshold && !diving)
            {
                if (acceleration >= -7.5f)
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
        else if (cM == "boostslide")
        {
            onGround = true;
            currentMove("BOOSTSLIDE");
        }
        else
        {

            onGround = true;
            currentMove("IDLE");
        }


        animator.SetBool("ANYSTATETRANSITION", anyStateTransition);
    }

    IEnumerator GetAcceleration(float t)
    {
        yield return new WaitForSeconds(0.1f);
        acceleration = (me.GetCurrentMove().GetHorizSpeedThisFrame().magnitude - t) / 0.1f;
    }

    IEnumerator AnyStateAgain(float t)
    {
        anyStateExtending++;
        
        yield return new WaitForSeconds(t);
        if (anyStateExtending == 1)
            anyStateTransition = true;

        anyStateExtending--;
    }

    IEnumerator ExtendMove(string move, float t)
    {
        extendedMove = move;
        movesExtending++;

        extending = true;
        yield return new WaitForSeconds(t);
        if (movesExtending == 1)
            extending = false;

        movesExtending--;


    }
}
