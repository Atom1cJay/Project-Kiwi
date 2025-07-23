using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    [SerializeField] List<Renderer> renderers;
    [SerializeField] float fadeTime;

    void OnTriggerEnter(Collider other)
    {
        // print("in");
        if (other.CompareTag("InvisibleCloudZone"))
        {
            StartCoroutine(Fade(false));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // print("out");
        if (other.CompareTag("InvisibleCloudZone"))
        {
            StartCoroutine(Fade(true));
        }
    }

    IEnumerator Fade(bool fadeIn)
    {
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            foreach (Renderer r in renderers)
            {
                float alpha = fadeIn ? Mathf.Lerp(0, 1, t / fadeTime) : Mathf.Lerp(1, 0, t / fadeTime);
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, alpha);
            }
            yield return null;
        }
    }
}
