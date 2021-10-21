using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ExperimentalObstacleHandler : MonoBehaviour
{
    [SerializeField] GameObject physicsCollider;

    private void Update()
    {
        HandleBumperMoved();
    }

    private void FixedUpdate()
    {
        HandleBumperMoved();
    }

    private void LateUpdate()
    {
        HandleBumperMoved();
    }

    public void HandleBumperMoved()
    {
        transform.Translate(physicsCollider.transform.localPosition);
        physicsCollider.transform.localPosition = Vector3.zero;
    }
}
