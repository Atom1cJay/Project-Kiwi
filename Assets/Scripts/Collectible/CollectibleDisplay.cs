using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Collectible;

public class CollectibleDisplay : MonoBehaviour
{
    [SerializeField] CollectibleType type;
    TextMeshProUGUI tm;

    // Start is called before the first frame update
    void Awake()
    {
        tm = GetComponent<TextMeshProUGUI>();

    }
}
