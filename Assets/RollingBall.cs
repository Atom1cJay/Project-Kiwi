using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBall : MonoBehaviour
{

    [SerializeField] float forceMultiplier, topForceMult;
    [SerializeField] GameObject player;
    MovingPlatformParenter mpp;
    Rigidbody rb;
    float nextTime;

    bool onBall, canDisengage, disengaged, justOff;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        onBall = false;
        canDisengage = false;
        disengaged = false;
        justOff = false;
        nextTime = -1f;
        mpp = player.GetComponent<MovingPlatformParenter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (justOff && Time.time < nextTime && canDisengage)
        {
            if(onBall)
            {
                canDisengage = false;
                Debug.Log("AAA fuck");
            }
            else
            {
                Debug.Log("AAA waiting");

            }
        }
        else
        {
            if(justOff && canDisengage && !disengaged)
            {
                justOff = false;
                Disengage();
                Debug.Log("AAA disengage");
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.layer == 9)
        {
            justOff = false;
            mpp.OnBall();
            Vector3 force = transform.position - other.transform.position;
            force *= forceMultiplier;

            //if you are below object, push. if above, reverse force
            if (transform.position.y - other.transform.position.y >= .125f)
            {
                force = new Vector3(force.x, 0, force.z);
                player.transform.parent = null;
                Debug.Log("AAA pushing");

            }
            else
            {
                force = new Vector3(-force.x * topForceMult, 0, -force.z * topForceMult);
                Debug.Log("AAA hop on");
                onBall = true;
                disengaged = false;
                canDisengage = true;
                player.transform.parent = transform;
            }

            rb.velocity = force;

        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == 9)
        {
            onBall = false;
            justOff = true;
            nextTime = Time.time + .25f;
        }
    }

    void Disengage()
    {
        mpp.OffBall();
        player.transform.parent = null;
        disengaged = true;
    }
}
