using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlatform : MonoBehaviour
{
    [SerializeField] float maxDistanceToMove, fallSpeed, riseSpeed, timeToFall;
    float lowestHeight, startingHeight, timeToCount, vertSpeed;
    Vector3 mvmtThisFrame;
    Vector3 rotThisFrame;
    bool playerOnPlatform, countingTime;

    private void Start()
    {
        vertSpeed = 0f;
        lowestHeight = transform.position.y - maxDistanceToMove;
        startingHeight = transform.position.y;
        timeToCount = 0f;
        playerOnPlatform = false;
        countingTime = false;
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
    /*

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !countingTime)
        {
            countingTime = true;
            timeToCount = Time.time + timeToFall;
            Debug.Log("doing thing");
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && Time.time >= tim && countingTime)
        {
            Debug.Log("start thing");
            playerOnPlatform = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            countingTime = false;
            playerOnPlatform = false;
            Debug.Log("off thing");
        }
    }*/

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
