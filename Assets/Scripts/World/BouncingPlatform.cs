using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed, randomRange;
    [SerializeField] bool maintainAngle;
    [SerializeField] Transform AngleTransform;
    [SerializeField] GameObject bouncingCollider;
    [SerializeField] Vector3 correctingVector;

    Vector3 mvmtVector;

    void Awake()
    {
        Vector3 random;
        if (randomRange != 0f)
            random = new Vector3(Random.Range(-randomRange, randomRange), 0f, Random.Range(-randomRange, randomRange));
        else
            random = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

        mvmtVector = random.normalized * speed;

        if (maintainAngle)
        {
            //t.eulerAngles = new Vector3(t.eulerAngles.x, Vector3.Angle(Vector3.zero, mvmtVector), t.eulerAngles.z);

            AngleTransform.rotation = Quaternion.LookRotation(mvmtVector);
            AngleTransform.eulerAngles = new Vector3(correctingVector.x, AngleTransform.eulerAngles.y + correctingVector.y, correctingVector.z);

        }

    }

    private void FixedUpdate()
    {
        transform.parent.Translate(mvmtVector * Time.fixedDeltaTime);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == bouncingCollider)
        {

            Vector3 normal = other.GetContact(0).normal;
            normal = new Vector3(normal.x, 0f, normal.z);
            Vector3 random = new Vector3(Random.Range(-randomRange, randomRange), 0f, Random.Range(-randomRange, randomRange));

            mvmtVector = (normal + random).normalized * speed;
            if (maintainAngle)
            {
                //t.eulerAngles = new Vector3(t.eulerAngles.x, Vector3.Angle(Vector3.zero, mvmtVector), t.eulerAngles.z);

                AngleTransform.rotation = Quaternion.LookRotation(mvmtVector);
                AngleTransform.eulerAngles = new Vector3(correctingVector.x, AngleTransform.eulerAngles.y + correctingVector.y, correctingVector.z);
                AngleTransform.transform.position = AngleTransform.transform.position + mvmtVector * 0.125f;
            }

        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject == bouncingCollider)
        {

            Vector3 normal = other.GetContact(0).normal;
            normal = new Vector3(normal.x, 0f, normal.z);
            Vector3 random = new Vector3(Random.Range(-randomRange, randomRange), 0f, Random.Range(-randomRange, randomRange));

            mvmtVector = (normal + random).normalized * speed;

            if (maintainAngle)
            {
                AngleTransform.rotation = Quaternion.LookRotation(mvmtVector);
                AngleTransform.eulerAngles = new Vector3(correctingVector.x, AngleTransform.eulerAngles.y + correctingVector.y, correctingVector.z);
                AngleTransform.transform.position = AngleTransform.transform.position + mvmtVector * 0.125f;

            }
            else
                transform.parent.position = transform.parent.position + mvmtVector * 0.5f;
        }
    }
}
