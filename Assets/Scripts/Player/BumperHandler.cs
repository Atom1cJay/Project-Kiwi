using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BumperHandler : MonoBehaviour
{
    [SerializeField] GameObject physicsCollider;

    public void HandleBumperMoved()
    {
        transform.Translate(physicsCollider.transform.localPosition);
        physicsCollider.transform.localPosition = Vector3.zero;
    }
}
