using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPlay : MonoBehaviour, ISoundController
{
    [SerializeField] string move, requiredPreviousMove, soundName;

    public void PlaySounds(string lastMove, string thisMove)
    {

        if (thisMove.Equals(move) && lastMove.Equals(requiredPreviousMove))
        {
            AudioMasterController.instance.PlaySound(soundName);
                
        }


    }
}
