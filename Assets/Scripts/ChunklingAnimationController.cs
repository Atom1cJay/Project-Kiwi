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
        if (waddle)
        {
            animator.SetBool("IDLE", false);
            animator.SetBool("RUNNING", true);
            animator.speed = speed;
        }
        else
        {
            animator.SetBool("IDLE", true);
            animator.SetBool("RUNNING", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
