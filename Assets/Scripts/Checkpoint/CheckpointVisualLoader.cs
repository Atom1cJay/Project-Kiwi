using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointVisualLoader : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] Object[] objectsToPause;
    [SerializeField] float timeToDisplay;
    [SerializeField] ParticleSystem PS;


    private void Start()
    {
        disablePlayer();
        startCheckpoint();
    }

    public void startCheckpoint()
    {
        animator.SetBool("START", true);

       // tempVisual.SetActive(false);
    }

    void disablePlayer()
    {
        foreach (Object obj in objectsToPause)
        {
            if (obj is GameObject)
                ((GameObject)obj).SetActive(false);
            if (obj is Component)
                SetComponent(false, ((Component)obj));
        }
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
        foreach (Object obj in objectsToPause)
        {
            if (obj is GameObject)
                ((GameObject)obj).SetActive(true);
            if (obj is Component)
                SetComponent(true, (Component)obj);
        }
    }

    void SetComponent(bool active, Component component)
    {
        if (component is Renderer)
        {
            (component as Renderer).enabled = active;
        }
        else if (component is MonoBehaviour)
        {
            (component as MonoBehaviour).enabled = active;
        }
        else if (component is Collider)
        {
            (component as Collider).enabled = active;
        }
        else if (component is Behaviour)
        {
            (component as Behaviour).enabled = active;
        }
    }
}
