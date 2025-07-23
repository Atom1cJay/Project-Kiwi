using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuitter : MonoBehaviour
{
    public void Quit()
    {
        print("Quitting");
        Application.Quit();
    }
}
