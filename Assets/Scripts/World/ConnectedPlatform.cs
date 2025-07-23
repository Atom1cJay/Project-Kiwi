using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedPlatform : MonoBehaviour
{
    [SerializeField] MoveExecuter me;
    [SerializeField] int obj1NotchMin, obj1NotchMax;
    [SerializeField] float notchHeight;
    [SerializeField] GameObject object1, object2, button;
    [SerializeField] float stompDuration;
    int obj1Notch;
    bool canPoundAgain;
    enum StompMode
    {
        NotStomping,
        Stomping1,
        Stomping2,
        Reseting
    };
    StompMode curStompMode;
    float stompElapsed;
    float goalPos1;
    float goalPos2;
    Vector3 obj1InitialPos, obj2InitialPos;

    // Start is called before the first frame update
    void Start()
    {
        if (obj1NotchMin > 0 || obj1NotchMax < 0)
            Debug.LogError("obj1NotchMin/Max badly configured");
        if (stompDuration <= 0)
            Debug.LogError("Connected platform cannot function with stomp duration of 0!");
        canPoundAgain = true;
        obj1Notch = 0;

        obj1InitialPos = object1.transform.position;
        obj2InitialPos = object2.transform.position;
    }

    void EndPound()
    {
        object1.transform.position = new Vector3(object1.transform.position.x, goalPos1, object1.transform.position.z);
        object2.transform.position = new Vector3(object2.transform.position.x, goalPos2, object2.transform.position.z);
        stompElapsed = 0;
        curStompMode = StompMode.NotStomping;
        canPoundAgain = true;
    }

    // Lower the first object and raise the second object
    public void initiateStompOne()
    {
        if (!canPoundAgain) // Current pound still going
        {
            return;
        }
        if (obj1Notch <= obj1NotchMax && obj1Notch > obj1NotchMin)
        {
            stompElapsed = 0;
            canPoundAgain = false;
            curStompMode = StompMode.Stomping1;
            goalPos1 = object1.transform.position.y - notchHeight;
            goalPos2 = object2.transform.position.y + notchHeight;
            obj1Notch--;
        }
    }

    // Lower the second object and raise the first object
    public void initiateStompTwo()
    {
        if (!canPoundAgain) // Current pound still going
        {
            return;
        }
        if (obj1Notch < obj1NotchMax && obj1Notch >= obj1NotchMin)
        {
            stompElapsed = 0;
            canPoundAgain = false;
            curStompMode = StompMode.Stomping2;
            goalPos1 = object1.transform.position.y + notchHeight;
            goalPos2 = object2.transform.position.y - notchHeight;
            obj1Notch++;
        }
    }

    // Reset the positions
    public void initiateReset()
    {
        if (!canPoundAgain) // Current pound still going
        {
            return;
        }
        curStompMode = StompMode.Reseting;
        obj1Notch = 0;
        goalPos1 = obj1InitialPos.y;
        goalPos2 = obj2InitialPos.y;

    }
    private void FixedUpdate()
    {
        if (curStompMode == StompMode.NotStomping)
        {
            return;
        }
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
        else if (curStompMode == StompMode.Reseting)
        {
            stompElapsed += Time.fixedDeltaTime;
            object1.transform.position += new Vector3(0f, (goalPos1 - object1.transform.position.y) * Time.fixedDeltaTime / stompDuration, 0f);
            object2.transform.position += new Vector3(0f, (goalPos2 - object2.transform.position.y) * Time.fixedDeltaTime / stompDuration, 0f);
        }
        if (stompElapsed > stompDuration)
        {
            EndPound();
        }
    }
}
