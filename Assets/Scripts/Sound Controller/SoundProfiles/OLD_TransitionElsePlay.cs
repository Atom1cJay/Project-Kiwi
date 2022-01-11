using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionElsePlay : MonoBehaviour, ISoundController
{
    [SerializeField] string move, playOnEveryMoveOtherThan, soundName;

    public void PlaySounds(string lastMove, string thisMove)
    {

        if (thisMove.Equals(move) && !lastMove.Equals(move) && !lastMove.Equals(playOnEveryMoveOtherThan))
        {
            AudioMasterController.instance.PlaySound(soundName);
                
        }


    }
}
