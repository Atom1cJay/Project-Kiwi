using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingSounds : MonoBehaviour, ISoundController
{
    [SerializeField] string move, soundNamePrimary, soundNameAlt;
    [SerializeField] MoveExecuter me;
    [SerializeField] float nextStep;

    bool primary = true;
    float distanceCovered = 0f;
    public void PlaySounds(string lastMove, string thisMove)
    {

        float speed = me.GetCurrentMove().GetHorizSpeedThisFrame().magnitude;


        if (thisMove.Equals(move))
        {
        distanceCovered += speed * Time.deltaTime;
            if (distanceCovered >= nextStep)
            {
                distanceCovered = 0f;
                if (primary)
                {
                    AudioMasterController.instance.PlaySound(soundNamePrimary);
                }
                else
                {
                    AudioMasterController.instance.PlaySound(soundNameAlt);
                }
                primary = !primary;

            }
        }
        else
        {
            primary = true;
            distanceCovered = nextStep;
        }


    }
}
