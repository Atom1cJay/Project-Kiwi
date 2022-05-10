using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownComponent : MonoBehaviour
{
    public Transform up;
    public Transform down;
    public int groupID;

    private void Start()
    {
        transform.position = UpDownManager.GroupStartsUp(groupID) ? up.position : down.position;
        UpDownManager.Register(this);
    }
}
