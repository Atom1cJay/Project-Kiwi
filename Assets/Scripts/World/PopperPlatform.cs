using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopperPlatform : MonoBehaviour
{
    [SerializeField] PopperPlatformGroup group;
    [SerializeField] Vector3 popRotation;
    [SerializeField] float moveTime;
    [SerializeField] bool doInitialMove;
    [SerializeField] bool poppedInEditor;
    [SerializeField] Renderer matRenderer;
    bool isPopped;

    private void Awake()
    {
        if (group == null)
        {
            Debug.LogError("Popper Platforms need a group! No group found.");
        }
        else
        {
            group.RegisterPlatform(this);
        }
    }

    void Start()
    {
        isPopped = poppedInEditor;
        // Simulate the first move having happened already
        if (doInitialMove)
        {
            // Choose direction to go
            Vector3 rotDest = !isPopped ?
                new Vector3(popRotation.x, popRotation.y, popRotation.z)
                :
                new Vector3(-popRotation.x, -popRotation.y, -popRotation.z);
            transform.Rotate(rotDest);
            isPopped = !isPopped;
        }
    }

    public void SwitchState()
    {
        StartCoroutine(SwitchStateMovement(!isPopped));
        isPopped = !isPopped;
        // TODO test
        if (isPopped)
        {
            matRenderer.material.SetColor("Albedo", Color.green);
        }
        else
        {
            matRenderer.material.SetColor("Albedo", Color.red);
        }
    }

    IEnumerator SwitchStateMovement(bool toPop)
    {
        Quaternion rotDest = toPop ?
            transform.rotation * Quaternion.Euler(popRotation.x, popRotation.y, popRotation.z)
            :
            transform.rotation * Quaternion.Euler(-popRotation.x, -popRotation.y, -popRotation.z);
        float time = 0;
        while (time < moveTime)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotDest, Time.deltaTime * 90 / moveTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
