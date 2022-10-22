using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioSystem : MonoBehaviour
{


    [SerializeField] MoveExecuter me;
    [SerializeField] EventSystem eventSystem;
    bool moveChanged, diving, startRunning, stopping, leftFootstep;
    string lM, temp, cM;
    float vertSpeed, jetrunThreshold, fallThreshold;

    float acceleration = 0f;
    [SerializeField] float nextStep;
    float distanceCovered = 0f;
    float lastSpeed = 0f;


    AudioMasterController amc = AudioMasterController.instance;

    GameObject selected;
    // Start is called before the first frame update
    void Start()
    {
        leftFootstep = true;
        //selected = eventSystem.currentSelectedGameObject;
        moveChanged = false;
        diving = false;
        startRunning = false;
        stopping = false;
    }


    void Update()
    {

        //actual movement controller

        #region Movement
        //get speeds
        float speed = me.GetCurrentMove().GetHorizSpeedThisFrame().magnitude;
        StartCoroutine(GetAcceleration(speed));
        temp = me.GetCurrentMove().AsString();
        vertSpeed = me.GetCurrentMove().GetVertSpeedThisFrame();

        distanceCovered += speed * Time.deltaTime;

        moveChanged = false;

        //move changed
        if (temp != cM)
        {
            lM = cM;
            cM = temp;
            moveChanged = true;
        }

        if (cM != "run")
        {
            leftFootstep = true;
        }

        if (cM == "idle")
        {
            //idle
            startRunning = true;
           if (stopping)
            {
                stopping = false;
                //stopping
            }


        }
        else if (cM == "fall")
        {
            //falling
        }
        else if (cM == "run")
        {
            //running
            if (distanceCovered >= nextStep)
            {
                distanceCovered = 0f;
                if (leftFootstep)
                {
                    amc.PlaySound("LFootstep");
                }
                else
                {
                    amc.PlaySound("RFootstep");
                }
                leftFootstep = !leftFootstep;
                
            }

        }
        else if (cM == "dive")
        {
            //diving
            playOnlyOnFirst("Dive");
        }
        else if (cM == "boostslidehop")
        {
            //boost slide jump
        }
        else if (cM == "jump")
        {
            //jump
            playOnlyOnFirst("Jump1");
        }
        else if (cM == "doublejump")
        {
            //second jump
            playOnlyOnFirst("Jump2");
        }
        else if (cM == "triplejump")
        {
            //triple jump
            playOnlyOnFirst("Jump3");
        }
        else if (cM == "horizairboostcharge")
        {
            //horizairboostcharge
            playOnlyOnFirst("JetCharge");
        }
        else if (cM == "horizairboost")
        {
            playOnlyOnFirst("JetExplosion");
            AudioMasterController.instance.StopSound("JetCharge");
        }
        else if (cM == "horizgroundboostcharge")
        {
            playOnlyOnFirst("JetCharge");
        }
        else if (cM == "horizgroundboost")
        {
            playOnlyOnFirst("JetExplosion");
            AudioMasterController.instance.StopSound("JetCharge");
        }
        else if (cM == "vertairboostcharge")
        {
            playOnlyOnFirst("JetCharge");
        }
        else if (cM == "vertairboost")
        {
            playOnlyOnFirst("JetExplosion");
            AudioMasterController.instance.StopSound("JetCharge");
        }
        else if (cM == "hardturn")
        {
        }

        else if (cM == "groundpound")
        {
        }
        else if (cM == "slide")
        {
        }
        else if (cM == "sliderecovery")
        {
        }
        else if (cM == "swim")
        {

        }
        else if (cM == "boostslide")
        {
            if (!AudioMasterController.instance.isPlaying("JetExplosion"))
                playOnlyOnFirst("JetExplosion");
            AudioMasterController.instance.StopSound("JetCharge");
        }
        else if (cM == "knockback")
        {
        }
        else if (cM == "glide")
        {
        }
        else
        {

        }

        #endregion

        GameObject tempSelected = new GameObject();

        /*
        if (eventSystem.currentSelectedGameObject)
        tempSelected = eventSystem.currentSelectedGameObject;
        

        bool changeInSelected = !selected.Equals(tempSelected);

        selected = tempSelected;
        Debug.Log("selected: " + selected);

        if (changeInSelected)
            AudioMasterController.instance.PlaySound("MenuNavigation");
        */
    }


    IEnumerator GetAcceleration(float t)
    {
        yield return new WaitForSeconds(0.1f);
        acceleration = (me.GetCurrentMove().GetHorizSpeedThisFrame().magnitude - t) / 0.1f;
    }

    void playOnlyOnFirst(string sound)
    {
        if (moveChanged)
        {
            AudioMasterController.instance.PlaySound(sound);
        }
    }
}