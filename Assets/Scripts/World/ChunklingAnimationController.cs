using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunklingAnimationController : MonoBehaviour
{
    [SerializeField] bool waddle;
    [SerializeField] float speed;
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Toggle(bool w)
    {
        waddle = w;

        if (waddle)
        {
            animator.SetBool("IDLE", false);
            animator.SetBool("RUNNING", true);
        }
        else
        {
            animator.SetBool("IDLE", true);
            animator.SetBool("RUNNING", false);
        }
    }
}
