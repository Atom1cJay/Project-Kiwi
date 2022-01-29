using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystemViewer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<LeafFallScript> leaves;
    [SerializeField] List<Image> images;
    [SerializeField] List<Color> colors;

    PlayerHealth ph;
    int currentHealth;

    private void Start()
    {
        ph = PlayerHealth.instance;
        currentHealth = ph.hp;
    }

    // Update is called once per frame
    void Update()
    {
        int temp = ph.hp;
        //Debug.Log(temp + ", cur: " + currentHealth);

        if (temp == currentHealth - 1)
        {
            if (leaves.Count > temp)
            {
                leaves[temp].DropLeaf();
            }
        }
        else if (temp == currentHealth + 1)
        {
            if (leaves.Count > temp)
            {
                leaves[temp].HealLeaf();
            }
        }

        if (temp <= colors.Count)
        {
            foreach (Image i in images)
            {
                Color c = colors[temp - 1];
                i.color = new Color(c.r, c.g, c.b, i.color.a);
            }
        }

        currentHealth = temp;
    }
}
