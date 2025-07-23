using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharlesController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] bool running = false;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("RUNNING", running);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
