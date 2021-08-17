using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed, randomRange;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 random = new Vector3(Random.Range(-randomRange, randomRange), 0f, Random.Range(-randomRange, randomRange));
        rb.velocity = random.normalized * speed;
    }

    private void FixedUpdate()
    {
        if(rb.velocity.magnitude < speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 9)
        {

            Vector3 normal = other.contacts[0].normal;
            normal = new Vector3(normal.x, 0f, normal.z);
            Vector3 random = new Vector3(Random.Range(-randomRange, randomRange), 0f, Random.Range(-randomRange, randomRange));


            rb.velocity = (normal + random).normalized * speed;

        }
    }
}
