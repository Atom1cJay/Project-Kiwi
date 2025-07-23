using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableBall : MonoBehaviour
{
    [SerializeField] float forcePower;
    [SerializeField] float vertForceAngle; // In radians
    [SerializeField] MoveExecuter me;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        Vector2 speed = MoveExecuter.instance.GetCurrentMove().GetHorizSpeedThisFrame();
        float horizForceAngle = Mathf.Atan2(speed.y, speed.x);
        float horizForceMultiplier = speed.magnitude;
        if (other.gameObject.layer == 9)
        {
            Vector3 force = new Vector3(
                Mathf.Cos(horizForceAngle) * forcePower * horizForceMultiplier,
                Mathf.Sin(vertForceAngle) * forcePower * horizForceMultiplier,
                Mathf.Sin(horizForceAngle) * forcePower * horizForceMultiplier);
            rb.velocity = force;
            print(force);
        }
    }
}
