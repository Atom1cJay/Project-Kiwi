using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectScript : MonoBehaviour
{
    [SerializeField] GameObject objectToCreate;

    bool released = true;


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.T) && released)
        {
            released = false;
            Instantiate(objectToCreate, transform.position, Quaternion.identity);
        }

        if (!Input.GetKey(KeyCode.T))
        {
            released = true;
        }
    }
}
