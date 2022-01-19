using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystemViewer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] PlayerHealth ph;
    [SerializeField] List<LeafFallScript> leaves;
    int currentHealth;

    private void Awake()
    {
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

        currentHealth = temp;
    }
}
