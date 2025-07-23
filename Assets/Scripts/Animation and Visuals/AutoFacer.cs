using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFacer : MonoBehaviour
{
    GameObject cam;
    Vector3 ir;

    [SerializeField] bool freezeX, freezeY, freezeZ;
    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
        ir = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

        Quaternion q = Quaternion.LookRotation(-cam.transform.forward);
        Vector3 v = q.eulerAngles;
        if(freezeX)
            v = new Vector3(ir.x, v.y, v.z);

        if (freezeY)
            v = new Vector3(v.x, ir.y, v.z);

        if (freezeZ)
            v = new Vector3(v.x, v.y, ir.z);

        transform.eulerAngles = v;
        
    }
}
