using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotator : MonoBehaviour
{
    [SerializeField] float timeToRotate;
    [SerializeField] float defaultRotation;

    bool isRotating = false;

    public void startRotation(float degreeRotation = 0)
    {
        if (isRotating)
            return;

        if (degreeRotation == 0)
            degreeRotation = defaultRotation;

        StartCoroutine(RotateCoroutine(degreeRotation));
    }


    IEnumerator RotateCoroutine(float degrees)
    {
        isRotating = true;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, startRotation.eulerAngles.y + degrees, 0);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / timeToRotate;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }


        isRotating = false;
    }
}
