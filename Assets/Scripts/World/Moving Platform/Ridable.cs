using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// For a moving platform which can rotate at a constant speed
/// and move in the pattern of a sine wave. Moves in the Update() method instead
/// of the FixedUpdate() method.
/// </summary>
public class Ridable : MonoBehaviour
{
    Vector3 translationThisFrame;
    Quaternion startFrameRot;
    Quaternion endFrameRot;
    Vector3 rotThisFrame;
    GameObject phantom;
    [SerializeField] AMovingPlatform moverScript;
    public UnityEvent onTransformChange = new UnityEvent();

    void Update()
    {
        Vector3 origPos = transform.position;
        startFrameRot = transform.rotation;
        Vector3 startFrameRotEuler = transform.rotation.eulerAngles;
        moverScript.FrameStart();
        moverScript.Translate();
        moverScript.Rotate();
        translationThisFrame = transform.position - origPos;
        endFrameRot = transform.rotation;
        Vector3 endFrameRotEuler = transform.rotation.eulerAngles;
        rotThisFrame = endFrameRotEuler - startFrameRotEuler;
        onTransformChange.Invoke();
    }

    public Vector3 TranslationThisFrame()
    {
        return translationThisFrame;
    }

    public Vector3 RotThisFrame()
    {
        return rotThisFrame;
    }

    public Vector3 PlayerPosChangeFromRotThisFrame(Vector3 playerOrigPos)
    {
        transform.rotation = startFrameRot;
        phantom.transform.position = playerOrigPos;
        transform.rotation = endFrameRot;
        return phantom.transform.position - playerOrigPos;
    }

    public void Register()
    {
        phantom = new GameObject("Phantom");
        phantom.transform.position = transform.position;
        phantom.transform.parent = transform;
    }

    public void Deregister()
    {
        Destroy(phantom.gameObject);
        phantom = null;
    }
}
