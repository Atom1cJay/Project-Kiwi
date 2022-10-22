using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointVisualLoader : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] GameObject tempVisual;
    [SerializeField] float timeToDisplay;
    [SerializeField] ParticleSystem PS;


    private void Start()
    {
        startVisuals();
    }

    public void startVisuals()
    {
        animator.SetBool("START", true);
        tempVisual.SetActive(false);
    }

    public void finishAnimation()
    {
        animator.SetBool("START", false);
        Invoke("displayPlayer", timeToDisplay);
    }

    public void startParticles()
    {
        PS.Play();
    }

    void displayPlayer()
    {
        tempVisual.SetActive(true);
    }
}
