using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectableGroupText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] float timeToDisappear = 2;

    void Start()
    {
        textComponent.enabled = false;
    }

    public void Activate(int collected, int total)
    {
        textComponent.enabled = true;
        textComponent.text = $"{collected}/{total}";
        StopAllCoroutines();
        StartCoroutine("WaitToDisappear");
    }

    IEnumerator WaitToDisappear()
    {
        yield return new WaitForSeconds(timeToDisappear);
        textComponent.enabled = false;
    }
}
