using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceApplier : MonoBehaviour
{
    [SerializeField] Vector3 velocityToApply;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("poggers we triggered " + other.name);
        MoveExecuter executer = other.gameObject.GetComponent<MoveExecuter>();

        if (executer != null)
        {
            executer.addAdditionalVelocityThisFrame(velocityToApply);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        MoveExecuter executer = other.gameObject.GetComponent<MoveExecuter>();

        if (executer != null)
        {
            executer.addAdditionalVelocityThisFrame(velocityToApply);
        }
    }
}
