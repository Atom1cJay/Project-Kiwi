using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{


    [SerializeField] MoveExecuter me;
    bool moveChanged, diving, startRunning, stopping;
    string lM, temp, cM;
    float vertSpeed, jetrunThreshold, fallThreshold;

    float acceleration = 0f;
    float lastSpeed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        moveChanged = false;
        diving = false;
        startRunning = false;
        stopping = false;
    }


    void FixedUpdate()
    {
        //get speeds
        float speed = me.GetCurrentMove().GetHorizSpeedThisFrame().magnitude;
        StartCoroutine(GetAcceleration(speed));
        temp = me.GetCurrentMove().AsString();
        vertSpeed = me.GetCurrentMove().GetVertSpeedThisFrame();

        moveChanged = false;

        //move changed
        if (temp != cM)
        {
            lM = cM;
            cM = temp;
            moveChanged = true;
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
        }
        else if (cM == "dive")
        {
            //diving
        }
        else if (cM == "boostslidehop")
        {
            //boost slide jump
        }
        else if (cM == "jump")
        {
            //jump
        }
        else if (cM == "doublejump")
        {
            //second jump
        }
        else if (cM == "triplejump")
        {
            //triple jump
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