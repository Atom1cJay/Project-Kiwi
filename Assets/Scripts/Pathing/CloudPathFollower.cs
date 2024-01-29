using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPathFollower : PathFollower
{
    [SerializeField] float thunderSpeedMultiplier;
    [SerializeField] float thunderSpeedDuration;
    [Header ("Landing Effect")]
    [SerializeField] MeshRenderer cloudRenderer;
    [SerializeField] float timeToLerpIn, timeToLerpOut;
    [SerializeField] Color landingCloudColor;

    Color originalCloudColor;
    bool inThunderSpeed;

    private void Awake()
    {
        originalCloudColor = cloudRenderer.materials[0].GetColor("Albedo");
    }

    public void ActivateThunderSpeed()
    {
        if (inThunderSpeed) return;
        StartCoroutine(ThunderSpeed());
    }

    IEnumerator ThunderSpeed()
    {
        // Initial Burst
        float initSpeed = speed;
        speed *= thunderSpeedMultiplier;
        float boostedSpeed = speed;
        inThunderSpeed = true;
        // Slowdown
        for (float t = 0; t < thunderSpeedDuration; t += Time.deltaTime)
        {
            speed = Mathf.Lerp(boostedSpeed, initSpeed, t / thunderSpeedDuration);
            yield return null;
        }
        speed = initSpeed;
        inThunderSpeed = false;
    }

    public void changeToLandingColor(float sec)
    {
        StartCoroutine(changeLandingColor(sec));
    }

    IEnumerator changeLandingColor(float pause)
    {
        Debug.Log("pos");
        float t = 0f;
        while (t < timeToLerpIn)
        {
            cloudRenderer.materials[0].SetColor("Albedo", Color.Lerp(originalCloudColor, landingCloudColor, t / timeToLerpIn));
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(pause);

        t = 0f;

        while (t < timeToLerpOut)
        {
            cloudRenderer.materials[0].SetColor("Albedo", Color.Lerp(landingCloudColor, originalCloudColor, t / timeToLerpOut));
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }
}
