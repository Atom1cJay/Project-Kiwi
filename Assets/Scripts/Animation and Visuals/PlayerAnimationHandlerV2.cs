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
    float timeToDance, startIdleTime;
    bool resetSlowDown = true;
    bool extending, anyStateTransition, boostSliding, startingIdle;
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
        animator.SetBool("SLIDETORUN", false);
        animator.SetBool("SLIDEJUMP", false);
        animator.SetBool("GROUNDHBOOST", false);
        animator.SetBool("GLIDING", false);
        animator.SetBool("KNOCKBACK", false);

        animator.SetBool(s, true);
    }
    void Start()
    {
        //initialize vars
        diving = false;
        stopping = false;
        extending = false;
        startRunning = false;
        boostSliding = false;
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
        animator.SetBool("SLIDETORUN", false);
        animator.SetBool("SLIDEJUMP", false);
        animator.SetBool("GROUNDHBOOST", false);
        animator.SetBool("GLIDING", false);
        animator.SetBool("KNOCKBACK", false);

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Idle")
            {
                startIdleTime = clip.length;
            }
        }

        startingIdle = true;

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

        //just for idle dance animation
        if (cM != "idle")
        {
            timeToDance = Time.time + 100f;
        }


       
        if (extending)
        {
            if(extendedMove == "STARTRUN" && cM == "jump")
            {
                anyStateTransition = true;
                extending = false;
            }
            currentMove(extendedMove);
        }
        else if (startRunning && speed < 0.5f && me.GetCurrentMove().GetVertSpeedThisFrame() == 0f && onGround)
        {

            boostSliding = false;
            onGround = true;
            currentMove("IDLE");
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
            else if (boostSliding)
            {
                StartCoroutine(ExtendMove("BOOSTSLIDE", 0.05f));
                currentMove("BOOSTSLIDE");
            }
            else if (stopping)
            {
                stopping = false;
                StartCoroutine(ExtendMove("STOPPING", 0.1f));
                currentMove("STOPPING");
            }
            else
            {

                currentMove("IDLE");
                /*
                if (Time.time > timeToDance - .25f)
                {
                    animator.SetBool("IDLE", false);
                    startingIdle = true;
                    
                }
                else if (startingIdle)
                {
                    currentMove("IDLE");
                    startingIdle = false;
                    timeToDance = Time.time + startIdleTime;
                }*/

            }


            boostSliding = false;
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
            else if (boostSliding)
            {
                boostSliding = false;
                anyStateTransition = false;
                StartCoroutine(AnyStateAgain(0.4f));
                StartCoroutine(ExtendMove("SLIDETORUN", 0.35f));
            }
            else if (acceleration <= -15f && speed >= jetrunThreshold * 0.3f && !diving)
            {
                
                currentMove("STOPPING");
                stopping = true;
            }
            else if ((lM == "idle" || lM == "hardturn") && startRunning)
            {
                
                currentMove("STARTRUN");
                anyStateTransition = false;
                StartCoroutine(AnyStateAgain(0.3f));
                StartCoroutine(ExtendMove("STARTRUN", 0.2f));
                startRunning = false;
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
            boostSliding = false;
            onGround = false;
            diving = true;
            currentMove("DIVE");

        }
        else if (cM == "boostslidehop")
        {
            onGround = false;
            boostSliding = false;
            anyStateTransition = false;
            StartCoroutine(AnyStateAgain(0.25f));
            StartCoroutine(ExtendMove("SLIDEJUMP", 0.3f));

        }
        else if (cM == "jump")
        {
            onGround = false;
            boostSliding = false;
            if (vertSpeed <= fallThreshold)
                currentMove("FALLING");
            else
            {
                currentMove("FIRSTJUMP");
            }

        }
        else if (cM == "doublejump")
        {
            boostSliding = false;
            onGround = false;
            currentMove("FIRSTJUMP"); // Temp, while animations are same

            if (vertSpeed <= fallThreshold)
                currentMove("FALLING");

        }
        else if (cM == "triplejump")
        {

            boostSliding = false;
            onGround = false;
            currentMove("THIRDJUMP");

            if (vertSpeed <= fallThreshold)
                currentMove("FALLING");

        }
        else if (cM == "horizairboostcharge")
        {
            boostSliding = false;
            onGround = false;
            currentMove("HCHARGE");

        }
        else if (cM == "horizairboost")
        {
            boostSliding = false;
            onGround = false;
            currentMove("HBOOST");

        }
        else if (cM == "horizgroundboostcharge")
        {
            boostSliding = false;
            onGround = true;
            currentMove("GROUNDHBOOST");
            anyStateTransition = false;
            StartCoroutine(AnyStateAgain(0.25f));
            StartCoroutine(ExtendMove("GROUNDHBOOST", 0.1f));
        }
        else if (cM == "horizgroundboost")
        {
            boostSliding = false;
            onGround = true;
            currentMove("JETPACKRUN");
        }
        else if (cM == "vertairboostcharge")
        {
            boostSliding = false;
            onGround = false;
            currentMove("VCHARGE");

        }
        else if (cM == "vertairboost")
        {
            boostSliding = false;
            onGround = false;
            currentMove("VBOOST");

        }
        else if (cM == "idle")
        {

            boostSliding = false;
            onGround = true;
            currentMove("IDLE");
        }
        else if (cM == "hardturn")
        {
            extending = false;
            boostSliding = false;
            startRunning = true;
            onGround = true;
            currentMove("HARDTURN");
            anyStateTransition = false;
            StartCoroutine(AnyStateAgain(0.4f));
        }

        else if (cM == "groundpound")
        {
            boostSliding = false;

            onGround = false;
            currentMove("GROUNDPOUND");
        }
        else if (cM == "slide")
        {

            boostSliding = false;
            onGround = true;
            currentMove("SKID");
        }
        else if (cM == "sliderecovery")
        {

            boostSliding = false;
            onGround = true;
            currentMove("SKID");
        }
        else if (cM == "swim")
        {

            boostSliding = false;
            onGround = true;
            currentMove("SWIM");
        }
        else if (cM == "boostslide")
        {
            onGround = true;
            boostSliding = true;
            currentMove("BOOSTSLIDE");
        }
        else if (cM == "knockback")
        {
            onGround = false;
            boostSliding = false;
            currentMove("KNOCKBACK");
        }
        else if (cM == "glide")
        {
            onGround = false;
            boostSliding = false;
            currentMove("GLIDING");
        }
        else
        {

            boostSliding = false;
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
