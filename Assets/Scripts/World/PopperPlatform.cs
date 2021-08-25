using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopperPlatform : MonoBehaviour
{
    [SerializeField] PopperPlatformGroup group;
    [SerializeField] Vector3 popRotation;
    [SerializeField] float moveTime;
    [SerializeField] bool startsAfterMove;
    Vector3 initialRot; // The first rotation, assuming the start is before the move
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
        initialRot = transform.rotation.eulerAngles;
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
        Vector3 rot1 = transform.rotation.eulerAngles;
        Vector3 rot2 = toPop ?
            transform.rotation.eulerAngles + new Vector3(popRotation.x, popRotation.y, popRotation.z)
            :
            initialRot;
        float time = 0;
        while (time < moveTime)
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(rot1, rot2, time / moveTime));
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(rot2);
    }
}
