using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopperPlatform : MonoBehaviour
{
    [SerializeField] PopperPlatformGroup group;
    [SerializeField] Vector3 popMovementLocal; // Relative to angle of object
    [SerializeField] float moveTime;
    [SerializeField] bool startsAfterMove;
    Vector3 initialPos; // The first position, assuming the start is before the move
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
        initialPos = transform.position;
        isPopped = startsAfterMove;
        // Simulate the first move having happened already
        if (startsAfterMove)
        {
            transform.position +=
                (transform.forward * popMovementLocal.z) +
                (transform.right * popMovementLocal.x) +
                (transform.up * popMovementLocal.y);
        }
    }

    public void SwitchState()
    {
        StartCoroutine(SwitchStateMovement(!isPopped));
        isPopped = !isPopped;
    }

    IEnumerator SwitchStateMovement(bool toPop)
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = toPop ?
            transform.position +
            (transform.forward * popMovementLocal.z) +
            (transform.right * popMovementLocal.x) +
            (transform.up * popMovementLocal.y)
            :
            initialPos;
        float time = 0;
        while (time < moveTime)
        {
            transform.position = Vector3.Lerp(pos1, pos2, time / moveTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = pos2;
    }
}
