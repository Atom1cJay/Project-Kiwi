using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopperPlatform : MonoBehaviour
{
    [SerializeField] PopperPlatformGroup group;
    [SerializeField] Vector3 popRotation;
    [SerializeField] float moveTime;
    [SerializeField] bool startsAfterMove;
    Quaternion initialRot; // The first rotation, assuming the start is before the move
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
        initialRot = transform.rotation;
        isPopped = startsAfterMove;
        // Simulate the first move having happened already
        if (startsAfterMove)
        {
            transform.Rotate(new Vector3(popRotation.x, popRotation.y, popRotation.z));
        }
    }

    public void SwitchState()
    {
        StartCoroutine(SwitchStateMovement(!isPopped));
        isPopped = !isPopped;
    }

    IEnumerator SwitchStateMovement(bool toPop)
    {
        Quaternion rot1 = transform.rotation;
        Quaternion rot2 = toPop ?
            transform.rotation * Quaternion.Euler(popRotation.x, popRotation.y, popRotation.z)
            :
            initialRot;
        float time = 0;
        while (time < moveTime)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot2, Time.deltaTime * 90 / moveTime);
            /*
            float x = Mathf.LerpAngle(rot1.x, rot2.x, time / moveTime);
            float y = Mathf.LerpAngle(rot1.y, rot2.y, time / moveTime);
            float z = Mathf.LerpAngle(rot1.z, rot2.z, time / moveTime);
            transform.eulerAngles = new Vector3(x, y, z);
            */
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //transform.rotation = rot2;
    }
}
