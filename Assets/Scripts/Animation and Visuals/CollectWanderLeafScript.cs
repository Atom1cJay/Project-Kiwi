using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectWanderLeafScript : MonoBehaviour
{
    
    [SerializeField] ParticleSystem PS;
    [SerializeField] MeshRenderer mr;
    [SerializeField] Rotator rot;

    [SerializeField] float startDuration, stayDuration, exitDuration, distanceToGoUp;

    bool started = false;
    bool collected = false;

    int animState = 0;

    public void startCollect()
    {
        collected = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (collected && !started)
        {
            started = true;
            animState = 1;
            //rot.slowToStop(startDuration / 2.5f);
            Invoke("startCollection", startDuration);

            GameObject.FindGameObjectWithTag("CameraTargetController")
                .GetComponent<CameraTarget>()
                .focusOnObject(gameObject.transform, startDuration + stayDuration);

            GameObject.FindGameObjectWithTag("WanderLeafCinematic")
                .GetComponent<CollectWanderLeafCinematic>()
                .enterCinematic(startDuration);
        }
        if (animState == 1)
        {
            transform.position += new Vector3(0f, (distanceToGoUp / startDuration) * Time.deltaTime, 0f);
        }
        if (animState == 2)
        {
                Color c = mr.material.color;
                mr.material.color = Color.Lerp(c, new Color(c.r, c.g, c.b, 0f), Time.deltaTime);

        }
    }

    void startCollection()
    {
        PS.Play();
        animState = 2;

        Invoke("startExit", stayDuration);
    }

    void startExit()
    {
        GameObject.FindGameObjectWithTag("WanderLeafCinematic")
            .GetComponent<CollectWanderLeafCinematic>()
            .exitCinematic(exitDuration);
        Destroy(gameObject, exitDuration + 5f);
    }
}
