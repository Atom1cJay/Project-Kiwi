using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystemV2 : MonoBehaviour
{
    ISoundController[] controllers;
    [SerializeField] MoveExecuter me;
    string lastMove = "";
    // Start is called before the first frame update
    void Awake()
    {
        controllers = GetComponentsInChildren<ISoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        string currentMove = me.GetCurrentMove().AsString();

        foreach (ISoundController c in controllers)
        {
            c.PlaySounds(lastMove, currentMove);
        }
        lastMove = currentMove;
    }
}
