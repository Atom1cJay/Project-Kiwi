using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectibleDisplay : MonoBehaviour
{
    TextMeshProUGUI tm;

    // Start is called before the first frame update
    void Awake()
    {
        tm = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void UpdateDisplay(int i)
    {
        tm.SetText(i.ToString());
    }
}
