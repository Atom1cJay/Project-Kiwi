using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed, randomRange;
    Vector3 mvmtVector;

    void Awake()
    {
        Vector3 random;
        if (randomRange != 0f)
            random = new Vector3(Random.Range(-randomRange, randomRange), 0f, Random.Range(-randomRange, randomRange));
        else
            random = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

        mvmtVector = random.normalized * speed;
    }

    private void FixedUpdate()
    {
        transform.parent.Translate(mvmtVector * Time.fixedDeltaTime);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 9)
        {
            Debug.Log(other.gameObject.name);

            Vector3 normal = other.GetContact(0).normal;
            normal = new Vector3(normal.x, 0f, normal.z);
            Vector3 random = new Vector3(Random.Range(-randomRange, randomRange), 0f, Random.Range(-randomRange, randomRange));


            mvmtVector = (normal + random).normalized * speed;

        }
    }
}
