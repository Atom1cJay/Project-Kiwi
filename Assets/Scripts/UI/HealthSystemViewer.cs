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

    [SerializeField] TMP_Text health;
    Color color;

    PlayerHealth ph;
    int currentHealth;

    private void Start()
    {

        ph = PlayerHealth.instance;
        currentHealth = ph.hp;
        color = colors[currentHealth - 1];
        foreach (Image image in images)
        {
            image.color = new Color(color.r, color.g, color.b, image.color.a);
        }
    }

    // Update is called once per frame
    void Update()
    {

        int temp = ph.hp;
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

        health.text = ""+currentHealth;
    }

}
