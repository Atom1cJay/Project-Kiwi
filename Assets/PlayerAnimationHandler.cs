using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] MoveExecuter me;
    //[SerializeField] MovementMaster mm;
    [SerializeField] Animator animator;

    void Start()
    {
        //Initialize all Animation variables as false
        animator.SetBool("JUMPING", false);
        animator.SetBool("WALKING", false);
        animator.SetBool("RUNNING", false);
        animator.SetBool("FALLING", false);
    }

    void FixedUpdate()
    {
        print(me.currentMoveAsString());

        //Jumping and Falling Animation Controllers
        /*
        if (mm.IsJumping())
        {
            animator.SetBool("JUMPING", true);
            animator.SetBool("FALLING", false);
        }
        else if (!mm.IsJumping() && !mm.IsOnGround())
        {
            animator.SetBool("JUMPING", false);
            animator.SetBool("FALLING", true);
        }
        else
        {
            animator.SetBool("JUMPING", false);
            animator.SetBool("FALLING", false);
        }

        //Walking Animation Controller
        if(mm.GetHorizSpeed() > 3f)
        {
            animator.SetBool("WALKING", false);
            animator.SetBool("RUNNING", true);
        }
        else if (mm.GetHorizSpeed() > 0.4f)
        {

            animator.SetBool("WALKING", true);
            animator.SetBool("RUNNING", false);
        }
        else
        {
            animator.SetBool("WALKING", false);
            animator.SetBool("RUNNING", false);
        }
        */
    }
}
