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
        UpDownManager.Register(this);
    }
}
