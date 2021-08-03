using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedPlatform : MonoBehaviour
{
    [SerializeField] MoveExecuter me;
    [SerializeField] int obj1NotchMin, obj1NotchMax;
    [SerializeField] float notchHeight;
    [SerializeField] GameObject object1, object2;
    [SerializeField] float stompDuration;
    int obj1Notch, obj2Notch;
    string temp;
    bool canPoundAgain;
    enum StompMode
    {
        NotStomping,
        Stomping1,
        Stomping2
    };
    StompMode curStompMode;
    float stompElapsed;
    float goalPos1;
    float goalPos2;

    // Start is called before the first frame update
    void Start()
    {
        if (obj1NotchMin > 0 || obj1NotchMax < 0)
            Debug.LogError("obj1NotchMin/Max badly configured");
        if (stompDuration <= 0)
            Debug.LogError("Connected platform cannot function with stomp duration of 0!");
        canPoundAgain = true;
        temp = ""; 
        obj1Notch = 0;
        obj2Notch = 0;
    }

    private void Update()
    {
        temp = me.GetCurrentMove().AsString();
        if (object1.transform.parent.Find("Player") != null && temp == "groundpound" && canPoundAgain)
            initiateStompOne();
        if (object2.transform.parent.Find("Player") != null && temp == "groundpound" && canPoundAgain)
            initiateStompTwo();
        
    }

    void EndPound()
    {
        object1.transform.position = new Vector3(object1.transform.position.x, goalPos1, object1.transform.position.z);
        object2.transform.position = new Vector3(object2.transform.position.x, goalPos2, object2.transform.position.z);
        stompElapsed = 0;
        curStompMode = StompMode.NotStomping;
        canPoundAgain = true;
    }

    //lower the first object and raise the second object
    void initiateStompOne()
    {
        if (obj1Notch <= obj1NotchMax && obj1Notch > obj1NotchMin)
        {
            stompElapsed = 0;
            canPoundAgain = false;
            curStompMode = StompMode.Stomping1;
            goalPos1 = object1.transform.position.y - notchHeight;
            goalPos2 = object2.transform.position.y + notchHeight;
            obj1Notch--;
            obj2Notch++;
        }
    }

    //lower the second object and raise the first object
    void initiateStompTwo()
    {
        if (obj1Notch < obj1NotchMax && obj1Notch >= obj1NotchMin)
        {
            stompElapsed = 0;
            canPoundAgain = false;
            curStompMode = StompMode.Stomping2;
            goalPos1 = object1.transform.position.y + notchHeight;
            goalPos2 = object2.transform.position.y - notchHeight;
            obj1Notch++;
            obj2Notch--;
        }
    }

    private void FixedUpdate()
    {
        if (curStompMode == StompMode.Stomping2)
        {
            stompElapsed += Time.fixedDeltaTime;
            object1.transform.position += new Vector3(0f, notchHeight * Time.fixedDeltaTime / stompDuration, 0f);
            object2.transform.position += new Vector3(0f, -notchHeight * Time.fixedDeltaTime / stompDuration, 0f);
        }
        else if (curStompMode == StompMode.Stomping1)
        {
            stompElapsed += Time.fixedDeltaTime;
            object1.transform.position += new Vector3(0f, -notchHeight * Time.fixedDeltaTime / stompDuration, 0f);
            object2.transform.position += new Vector3(0f, notchHeight * Time.fixedDeltaTime / stompDuration, 0f);
        }

        if (stompElapsed > stompDuration)
        {
            EndPound();
        }
    }
}
