using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoveringTextBox : MonoBehaviour
{
    [SerializeField] Canvas hoveringTextBoxCanvas;
    [SerializeField] Image hoveringBackground;
    [SerializeField] TextMeshProUGUI hoveringText;
    [SerializeField] float fadeTime;
    [SerializeField] float maxOpaqueness;

    private void Awake()
    {
        hoveringTextBoxCanvas.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine("AnimateIn");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine("AnimateOut");
        }
    }

    IEnumerator AnimateIn()
    {
        hoveringTextBoxCanvas.enabled = true;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            float alpha = (t / fadeTime) * maxOpaqueness;
            hoveringBackground.color = new Color(hoveringBackground.color.r, hoveringBackground.color.g, hoveringBackground.color.b, alpha);
            hoveringText.color = new Color(hoveringText.color.r, hoveringText.color.g, hoveringText.color.b, alpha);
            yield return null;
        }
    }

    IEnumerator AnimateOut()
    {
        float initialAlpha = hoveringBackground.color.a;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            float alpha = initialAlpha - ((t / fadeTime) * initialAlpha);
            hoveringBackground.color = new Color(hoveringBackground.color.r, hoveringBackground.color.g, hoveringBackground.color.b, alpha);
            hoveringText.color = new Color(hoveringText.color.r, hoveringText.color.g, hoveringText.color.b, alpha);
            yield return null;
        }
        hoveringTextBoxCanvas.enabled = false;
    }
}
