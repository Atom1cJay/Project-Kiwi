using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotation;

    //bool stopping = false;
    //float duration;

    void Update()
    {
        transform.localRotation *= Quaternion.Euler(rotation * Time.deltaTime);

        /*
        if (stopping)
        {
            rotation = Vector3.Lerp(rotation, Vector3.zero, (1f / duration) * Time.deltaTime);

            if (rotation.magnitude < 0.01f)
                Destroy(this);
        }
        */
    }

    /*
    public void slowToStop(float _duration)
    {
        duration = _duration;
        stopping = true;
    }
    */
}
