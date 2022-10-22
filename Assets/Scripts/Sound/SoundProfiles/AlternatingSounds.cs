using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingSounds : MonoBehaviour
{
    [SerializeField] string soundNamePrimary, soundNameAlt;
    [SerializeField] MoveExecuter me;
    [SerializeField] float nextStep;

    bool primary = true;
    float distanceCovered = 0f;

    private StateController StateChecker;

    private void Awake()
    {
        StateChecker = GetComponent<StateController>();
    }

    public void Update() {

        float speed = MoveExecuter.GetCurrentMove().GetHorizSpeedThisFrame().magnitude;


        if (StateChecker.StateCheck())
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
