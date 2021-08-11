using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ExperimentalObstacleHandler : MonoBehaviour
{
    [SerializeField] GameObject physicsCollider;

    private void FixedUpdate()
    {
       if (physicsCollider.transform.localPosition != Vector3.zero)
       {
            transform.Translate(physicsCollider.transform.localPosition);
            physicsCollider.transform.localPosition = Vector3.zero;
       }
    }
}
