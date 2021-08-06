using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlatform : MonoBehaviour
{
    [SerializeField] float maxDistanceToMove, fallSpeed, riseSpeed, timeToFall;
    float lowestHeight, startingHeight, vertSpeed;
    Vector3 mvmtThisFrame;
    Vector3 rotThisFrame;
    bool playerOnPlatform, countingTime;

    private void Start()
    {
        vertSpeed = 0f;
        lowestHeight = transform.position.y - maxDistanceToMove;
        startingHeight = transform.position.y;
        playerOnPlatform = false;
    }

    void Update()
    {
        playerOnPlatform = transform.parent.Find("Player") != null;
        GetDistanceToMove();
        mvmtThisFrame =
            new Vector3(
                0f,
               vertSpeed,
                0f) * Time.fixedDeltaTime;

    }

    void FixedUpdate()
    {
        transform.Translate(mvmtThisFrame);
    }

    void GetDistanceToMove()
    {
        if (transform.position.y < lowestHeight)
        {
            transform.position = new Vector3(transform.position.x, lowestHeight, transform.position.z);
            vertSpeed = 0f;
        }
        else if (transform.position.y > startingHeight)
        {

            transform.position = new Vector3(transform.position.x, startingHeight, transform.position.z);
            vertSpeed = 0f;
        }
        else {
            if (playerOnPlatform)
            {
                 vertSpeed += fallSpeed * Time.deltaTime;
            }
            else
            {
                vertSpeed += riseSpeed * Time.deltaTime;
            }
        }
    }
}
