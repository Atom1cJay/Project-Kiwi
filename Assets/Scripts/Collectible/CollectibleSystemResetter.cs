using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSystemResetter : MonoBehaviour
{
    public void Reset()
    {
        CollectibleSystem.ResetCollected();
    }
}
