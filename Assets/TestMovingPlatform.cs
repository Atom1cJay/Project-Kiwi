using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovingPlatform : MonoBehaviour
{
    [SerializeField] Vector3 distanceToMove;
    [SerializeField] float speed;
    [SerializeField] Vector3 rotSpeed;


    void FixedUpdate()
    {
        float waveValue = (Mathf.Sin(speed * Time.time) / 2);
        print(waveValue);

        transform.Translate(new Vector3(
            waveValue * distanceToMove.x,
            waveValue * distanceToMove.y,
            waveValue * distanceToMove.z)
            * Time.fixedDeltaTime);

        transform.Rotate(
            rotSpeed.x * Time.fixedDeltaTime,
            rotSpeed.y * Time.fixedDeltaTime,
            rotSpeed.z * Time.fixedDeltaTime);
    }
}
