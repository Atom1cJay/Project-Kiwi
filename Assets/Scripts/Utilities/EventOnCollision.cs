using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnCollision : MonoBehaviour
{
    [SerializeField] UnityEvent onCollision;
    [SerializeField] List<string> acceptableTags;
    [SerializeField] bool trigger;
    [SerializeField] bool oneTimeActivation;
    bool activatedOnce;

    void OnTriggerEnter(Collider other)
    {
        if ((!oneTimeActivation || !activatedOnce) && trigger && acceptableTags.Contains(other.tag))
        {
            onCollision.Invoke();
            activatedOnce = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if ((!oneTimeActivation || !activatedOnce) && !trigger && acceptableTags.Contains(other.collider.tag))
        {
            onCollision.Invoke();
            activatedOnce = true;
        }
    }
}
