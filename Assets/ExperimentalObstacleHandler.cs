using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ExperimentalObstacleHandler : MonoBehaviour
{
    CharacterController controller;
    float distance;
    int platLayer;
    [SerializeField] float skinLength;
    Vector3 toMove;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        //Distance is slightly larger than the
        distance = controller.radius + skinLength;

        //First add a Layer name to all platforms (I used MovingPlatform)
        //Now this script won't run on regular objects, only platforms.
        platLayer = LayerMask.NameToLayer("MovingPlatform");
    }

    public void Update()
    {
        toMove = Vector3.zero;
        RaycastHit hit;

        //Bottom of controller. Slightly above ground so it doesn't bump into slanted platforms. (Adjust to your needs)
        Vector3 p1 = transform.position + Vector3.up * 0.25f;
        //Top of controller
        Vector3 p2 = p1 + Vector3.up * controller.height;

        //Check around the character in a 360, 10 times (increase if more accuracy is needed)
        for (int i = 0; i < 360; i += 36)
        {
            //Check if anything with the platform layer touches this object
            if (Physics.CapsuleCast(p1, p2, 0, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit, distance, 1 << platLayer))
            {
                //If the object is touched by a platform, move the object away from it
                toMove += (hit.normal * (distance - hit.distance));
            }
        }

        //[Optional] Check the players feet and push them up if something clips through their feet.
        //(Useful for vertical moving platforms)
        if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hit, 1, 1 << platLayer))
        {
            toMove += (Vector3.up * (1 - hit.distance));
        }

        controller.Move(toMove);
    }
}
