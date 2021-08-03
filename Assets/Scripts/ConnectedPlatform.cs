using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedPlatform : MonoBehaviour
{
    [SerializeField] MoveExecuter me;
    [SerializeField] int obj1NotchLimit, obj2NotchLimit;
    [SerializeField] float notchHeight;
    [SerializeField] GameObject object1, object2;
    int obj1Notch, obj2Notch;
    float obj1y, obj2y;
    string temp;
    bool canPoundAgain;

    // Start is called before the first frame update
    void Start()
    {
        canPoundAgain = true;
        temp = ""; 
        obj1Notch = 0;
        obj2Notch = 0;
    }

    private void Update()
    {
        temp = me.GetCurrentMove().AsString();

        Debug.Log(obj1Notch + ", " + obj2Notch);
        if (object1.transform.parent.Find("Player") != null && temp == "groundpound" && canPoundAgain)
            stompTwo();
        if (object2.transform.parent.Find("Player") != null && temp == "groundpound" && canPoundAgain)
            stompOne();
        
    }

    void ResetPoundTimer()
    {
        canPoundAgain = true;
    }

    //lower the first object and raise the second object
    void stompOne()
    {
        Debug.Log("gp 1");
        canPoundAgain = false;
        Invoke("ResetPoundTimer", 0.5f);
        if (obj1Notch < obj1NotchLimit)
        {
            object1.transform.position -= new Vector3(0f, -notchHeight, 0f);
            object2.transform.position -= new Vector3(0f, notchHeight, 0f);
            obj1Notch++;
            obj2Notch--;
        }
    }

    //lower the second object and raise the first object
    void stompTwo()
    {
        Debug.Log("gp 2");
        canPoundAgain = false;
        Invoke("ResetPoundTimer", 0.5f);
        if (obj2Notch < obj2NotchLimit)
        {
            object1.transform.position -= new Vector3(0f, notchHeight, 0f);
            object2.transform.position -= new Vector3(0f, -notchHeight, 0f);
            obj1Notch--;
            obj2Notch++;
        }
    }

}
