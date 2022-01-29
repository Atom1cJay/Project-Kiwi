using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayAtStart : MonoBehaviour, ISoundController
{
    [SerializeField] string move, soundName;

    public void PlaySounds(string lastMove, string thisMove)
    {
        if (thisMove.Equals(move) && !lastMove.Equals(move))
        {
            AudioMasterController.instance.PlaySound(soundName, gameObject);
        }
    }
}