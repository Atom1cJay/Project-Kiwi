using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandMonster : MonoBehaviour
{
    [SerializeField] float patrolSpeed;
    [SerializeField] float angularSpeed;
    [SerializeField] float raycastHeight;
    [SerializeField] float heightOffGround; // To prevent clipping of particles
    float angle;

    private void Start()
    {
        //angle = Random.Range(0, 360);
    }

    private void Update()
    {
        // Basic Movement
        float xMovement = Mathf.Cos(angle);
        float zMovement = Mathf.Sin(angle);
        transform.position += new Vector3(xMovement, 0, zMovement) * patrolSpeed * Time.deltaTime;
        // Angle Adjustment
        angle += angularSpeed * Time.deltaTime;
        // Y Position Adjustment (to ground I'm on)
        Vector3 raycastOrigin = transform.position + (raycastHeight * Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + heightOffGround, transform.position.z);
        }
        else
        {
            Debug.LogError("Sand Monster: Raycast not finding any ground. This may be an inappropriate surface for it.");
        }
        //print(Physics.OverlapSphere(transform.position, 1));
    }
}
