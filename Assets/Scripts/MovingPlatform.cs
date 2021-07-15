using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector3 distanceToMove;
    [SerializeField] float speed;
    [SerializeField] Vector3 rotSpeed;
    Vector3 distToMove;
    Vector3 amtToRot;

    void Update()
    {
        float waveValue = (Mathf.Sin(speed * Time.time) / 2);

        distToMove =
            new Vector3(
                waveValue * distanceToMove.x,
                waveValue * distanceToMove.y,
                waveValue * distanceToMove.z)* Time.deltaTime;

        amtToRot =
            new Vector3(
                rotSpeed.x,
                rotSpeed.y,
                rotSpeed.z) * Time.deltaTime;
    }

    void FixedUpdate()
    {
        transform.Translate(distToMove);
        transform.Rotate(amtToRot);
    }
}
