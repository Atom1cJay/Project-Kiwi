using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerDecoration : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material[] materials;
    [SerializeField] float rotateRange = 5;
    [SerializeField] float rotateSpeed = 0.25f;
    [SerializeField] float randomizeRangeMult = 0.5f;
    [SerializeField] float randomizeSpeedMult = 0.35f;

    float actualRotateRange;
    float actualSpeedRange;


    Vector3 eulerAngles;

    private void Awake()
    {
        meshRenderer.material = materials[Random.Range(0, materials.Length)];
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Random.Range(0f, 360f));
        eulerAngles = transform.localEulerAngles;
        actualRotateRange = rotateRange * Random.Range(1f - randomizeRangeMult, 1f + randomizeRangeMult);
        actualSpeedRange = rotateSpeed * Random.Range(1f - randomizeSpeedMult, 1f + randomizeSpeedMult);
    }

    private void Update()
    {
        transform.localEulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, eulerAngles.z + Mathf.Sin(Time.time * actualRotateRange) * actualSpeedRange);
    }
}
