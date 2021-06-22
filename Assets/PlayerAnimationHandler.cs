using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] MoveExecuter me;
    //[SerializeField] MovementMaster mm;
    [SerializeField] Animator animator;
    bool onGround = false;
    bool diving = false;

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

        animator.SetBool(s, true);
    }
    void Start()
    {
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
    }

    void FixedUpdate()
    {
        string cM = me.currentMoveAsString();
        Debug.Log(cM);
        //walking state not implemented yet
        
        if (cM == "walking")
        {
            onGround = true;
            currentMove("WALKING");
        }
        else if (cM == "idle")
        {
            onGround = true;
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
            currentMove("RUNNING");
        }
        else if (cM == "dive")
        {
            diving = true;
            onGround = false;
            currentMove("DIVE");

        }
        else if (cM == "jump")
        {
            onGround = false;
            currentMove("FIRSTJUMP");

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
        else
        {

            onGround = true;
            currentMove("IDLE");
        }
    }
}
