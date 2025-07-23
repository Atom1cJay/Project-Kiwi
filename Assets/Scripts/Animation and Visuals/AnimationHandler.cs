using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] List<string> states;
    [SerializeField] Animator animator;

    float speed;

    private void Awake()
    {
        speed = 1.25f;

    }

    public void currentMove(string s)
    {
        for(int i = 0; i < states.Count; i++)
        {
            animator.SetBool(states[i], false);
        }

        animator.SetBool(s, true);
    }

    public void PauseAnimation()
    {
        speed = animator.speed;
        animator.speed = 0f;
    }

    public void ResumeAnimation()
    {
        animator.speed = speed;
    }
}
