using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HealthView : MonoBehaviour
{
    [SerializeField] string prefix; // Something like "Health: "
    [SerializeField] PlayerHealth ph;
    TextMeshProUGUI tm;

    void Start()
    {
        tm = GetComponent<TextMeshProUGUI>();
        ph.onHealthChanged.AddListener(() => UpdateDisplay());
        tm.SetText(prefix + ph.hp);
    }

    public void UpdateDisplay()
    {
        tm.SetText(prefix + ph.hp);
    }
}
