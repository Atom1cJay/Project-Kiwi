using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementInfo))]
public class MovingPlatformParenter : MonoBehaviour
{
    MovementInfo mi;

    bool onBall;

    void Start()
    {
        mi = GetComponent<MovementInfo>();
        onBall = false;
    }

    //Get on Ball
    public void OnBall()
    {
        onBall = true;
    }


    //Get off Ball
    public void OffBall()
    {
        onBall = false;
    }

    private void FixedUpdate()
    {
        
        if (!onBall)
        {
            Debug.Log("off ball!!!");
            if (!mi.TouchingGround())
            {
                transform.SetParent(null, true);
            }
            else
            {
                GameObject ground = mi.GetGroundDetector().CollidingWith();
                Transform parent;
                if (ground.CompareTag("Moving Platform (Has Wrapper)"))
                    parent = ground.transform.parent;
                else if (ground.CompareTag("Bouncing Platform"))
                {

                    parent = ground.transform;
                    Debug.Log("boomshakalaka");
                }
                else
                    parent = null;
                transform.SetParent(parent);
            }
        }
        else
        {
            Debug.Log("on ball!!!");
        }   
        
    }
}
