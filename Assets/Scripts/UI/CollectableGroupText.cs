using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectableGroupText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent.enabled = false;
    }

    public void Activate(int collected, int total)
    {
        textComponent.enabled = true;
        textComponent.text = $"{collected}/{total}";
        Invoke("Disappear", 2);
    }

    void Disappear()
    {
        textComponent.enabled = false;
    }
}
