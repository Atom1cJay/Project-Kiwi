using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPlay : MonoBehaviour, ISoundController
{
    [SerializeField] string soundName;
    [SerializeField] StateController check;

    public void PlaySounds(string lastMove, string thisMove)
    {

        if (check.StateCheck())
        {
            AudioMasterController.instance.PlaySound(soundName);
                
        }


    }
}
