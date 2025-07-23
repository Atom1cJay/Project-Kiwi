using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
    [SerializeField] float timeUntilDestroy;

    void Start()
    {
        Destroy(gameObject, timeUntilDestroy);
    }
}
