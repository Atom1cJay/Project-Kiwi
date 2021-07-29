using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopperPlatform : MonoBehaviour
{
    [SerializeField] Vector3 popMovementLocal; // Relative to angle of object
    [SerializeField] float moveTime;
    [SerializeField] float offTime;
    [SerializeField] bool startsAfterMove;
    Vector3 initialPos; // The first position, assuming the start is before the move

    void Start()
    {
        initialPos = transform.position;
        // Simulate the first move having happened already
        if (startsAfterMove)
        {
            transform.position +=
                (transform.forward * popMovementLocal.z) +
                (transform.right * popMovementLocal.x) +
                (transform.up * popMovementLocal.y);
        }
        StartCoroutine("Pop");
    }

    IEnumerator Pop()
    {
        while (true)
        {
            Vector3 pos1 = transform.position;
            Vector3 pos2;
            if ((pos1 - initialPos).magnitude < 0.01f)
            {
                pos2 = transform.position +
                (transform.forward * popMovementLocal.z) +
                (transform.right * popMovementLocal.x) +
                (transform.up * popMovementLocal.y);
            }
            else
            {
                pos2 = initialPos;
            }
            float time = 0;
            while (time < moveTime)
            {
                transform.position = Vector3.Lerp(pos1, pos2, time / moveTime);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            transform.position = pos2;
            yield return new WaitForSeconds(offTime);
        }
    }
}
