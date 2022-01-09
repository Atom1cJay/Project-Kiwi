using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundProfileController : MonoBehaviour
{
    [SerializeField] string move;
    [SerializeField] ISoundController controllers;

    string lastMove = "";

    public void UpdateMove(string input)
    {
        if (input.Equals(move) || lastMove.Equals(move))
        {

        }
        lastMove = input;
    }



}
