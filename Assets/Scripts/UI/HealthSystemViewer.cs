using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystemViewer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<LeafFallScript> leaves;
    [SerializeField] List<Image> images;
    [SerializeField] List<Color> colors;
    [SerializeField] float uiShakeTime = 0.4f;
    [SerializeField] float uiShakeStrength = 10;

    [SerializeField] TMP_Text health;
    Color color;

    PlayerHealth ph;
    int currentHealth;

    private void Start()
    {
        ph = PlayerHealth.instance;
        ph.onHealthChanged.AddListener(OnHealthChanged);
        currentHealth = ph.hp;
        color = colors[currentHealth - 1];
        foreach (Image image in images)
        {
            image.color = new Color(color.r, color.g, color.b, image.color.a);
        }
    }

    void OnHealthChanged()
    {
        int temp = ph.hp;
        if (temp <= 0) return;
        color = colors[temp - 1];

        if (temp == currentHealth - 1)
        {
            if (leaves.Count > temp)
            {
                leaves[temp].DropLeaf();
            }

            foreach (Image image in images)
            {
                image.color = new Color(color.r, color.g, color.b, image.color.a);
            }

        }
        else if (temp == currentHealth + 1)
        {
            if (leaves.Count > temp)
            {
                leaves[temp].HealLeaf();
            }
        }

        currentHealth = temp;

        health.text = "" + currentHealth;
        StartCoroutine(ShakeHealthUI());
    }

    IEnumerator ShakeHealthUI()
    {
        float t = 0;
        Vector3 initPos = transform.position;
        while (t < uiShakeTime)
        {
            Vector3 newDisplacement = new Vector3(Random.Range(-uiShakeStrength, uiShakeStrength), Random.Range(-uiShakeStrength, uiShakeStrength), 0);
            transform.position = initPos + newDisplacement;
            yield return null;
            t += Time.deltaTime;
        }
        transform.position = initPos;
    }
}
