using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcProjectile : MonoBehaviour
{
    [SerializeField] float rotationSpeedWhenLaunched = 5f;
    [SerializeField] GameObject targetMarker;
    [SerializeField] float timeToScaleUp = 0f;

    Rigidbody rb;
    GameObject targetVisuals;
    Vector3 initialScale;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();

        if (timeToScaleUp != 0f)
        {
            initialScale = transform.localScale;
            StartCoroutine("scaleToNormalSize");
        }
    }

    IEnumerator scaleToNormalSize()
    {
        transform.localScale = Vector3.zero;

        float time = 0f;
        while (time < timeToScaleUp)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, time / timeToScaleUp);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        transform.localScale = initialScale;
    }


    public void launchProjectileWithSpeed(Vector3 target, float horizontalSpeed)
    {
        if (targetMarker != null)
        {
            targetVisuals = Instantiate(targetMarker, target, targetMarker.transform.rotation);
        }

        Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 targetXZPos = new Vector3(target.x, 0f, target.z);

        float deltaX = target.x - transform.position.x;
        float deltaY = target.y - transform.position.y;
        float deltaZ = target.z - transform.position.z;

        float distance = Vector3.Distance(projectileXZPos, targetXZPos);

        float time = distance / horizontalSpeed;

        float gravity = Physics.gravity.y;

        float Vy = (deltaY - 0.5f * gravity * time * time) / time;

        Vector3 localVelocity = new Vector3(deltaX / time, Vy, deltaZ / time);

        rb.velocity = localVelocity;
        rb.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * rotationSpeedWhenLaunched;
    }

    public void launchProjectileWithAngle(Vector3 target, float angle)
    {
        Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 targetXZPos = new Vector3(target.x, 0f, target.z);

        float deltaY = target.y - transform.position.y;

        float gravity = Physics.gravity.y;

        float distance = Vector3.Distance(projectileXZPos, targetXZPos);

        float time = Mathf.Sqrt(((2f / gravity) * (deltaY - (Mathf.Tan(angle * Mathf.Deg2Rad) * distance))));

        launchProjectileWithSpeed(target, distance / time);
    }

    private void OnDestroy()
    {
        if (targetMarker != null)
        {
            Destroy(targetVisuals);
        }
    }
}
