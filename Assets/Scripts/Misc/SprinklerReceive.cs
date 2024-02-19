using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SprinklerReceive : MonoBehaviour
{

    [SerializeField] UnityEvent receiveSprinkle;

    public void receive()
    {
        receiveSprinkle.Invoke();
    }


}
