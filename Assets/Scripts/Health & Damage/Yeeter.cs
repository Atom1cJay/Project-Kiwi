using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Yeeter : MonoBehaviour
{
    [SerializeField] UnityEvent onYeet;
    [SerializeField] float yeetVelocity;
    [SerializeField] bool doDamage;
    public bool isActivated = true;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ConsiderYeet();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ConsiderYeet();
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ConsiderYeet();
        }
    }

    void ConsiderYeet()
    {
        if (isActivated)
        {
            PlayerHealth.instance.handleYeet(doDamage);

            YeetController.instance.initialYVelocity = yeetVelocity;

            onYeet.Invoke();
        }
    }
}
