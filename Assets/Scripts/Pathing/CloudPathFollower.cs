using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPathFollower : PathFollower
{
    [SerializeField] float thunderSpeedMultiplier;
    [SerializeField] float thunderSpeedDuration;
    [Header ("Landing Effect")]
    [SerializeField] MeshRenderer cloudRenderer;
    [SerializeField] float timeToLerpInLand, timeToLerpOutLand;
    [SerializeField] Color landingCloudColor;
    [SerializeField] Sound landingSound;
    [Header ("Lightening Effect")]
    [SerializeField] float timeToLerpInLightening;
    [SerializeField] float timeToLerpOutLightening;
    [SerializeField] float lighteningDuration;
    [SerializeField] float lightneningEffectDistance;
    [SerializeField] Color lighteningCloudColor;

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
        AudioMasterController.instance.PlaySound(landingSound);
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
        StartCoroutine(changeLandingColor(sec, timeToLerpInLand, timeToLerpOutLand, landingCloudColor));
    }

    IEnumerator changeLandingColor(float pause, float timeToLerpIn, float timeToLerpOut, Color color)
    {
        float t = 0f;
        while (t < timeToLerpIn)
        {
            cloudRenderer.materials[0].SetColor("Albedo", Color.Lerp(originalCloudColor, color, t / timeToLerpIn));
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        cloudRenderer.materials[0].SetColor("Albedo", color);

        yield return new WaitForSeconds(pause);

        t = 0f;

        while (t < timeToLerpOut)
        {
            cloudRenderer.materials[0].SetColor("Albedo", Color.Lerp(color, originalCloudColor, t / timeToLerpOut));
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        cloudRenderer.materials[0].SetColor("Albedo", originalCloudColor);

    }

    public void activateLightening()
    {
        StopAllCoroutines();

        List<GameObject> lightningReceiverObjects = new List<GameObject>();
        LightningReceiver[] lightningReceivers = FindObjectsOfType<LightningReceiver>();


        foreach (LightningReceiver receiver in lightningReceivers)
        {
            float distance = Vector3.Distance(transform.position, receiver.transform.position);
            if (distance <= lightneningEffectDistance)
            {
                receiver.receive();
            }
        }

        StartCoroutine(changeLandingColor(lighteningDuration, timeToLerpInLightening, timeToLerpOutLightening, lighteningCloudColor));

    }
}
