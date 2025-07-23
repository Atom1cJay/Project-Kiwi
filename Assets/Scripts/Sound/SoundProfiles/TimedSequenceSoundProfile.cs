using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound Profile/Timed Sequence")]
public class TimedSequenceSoundProfile : SoundProfile
{
    [System.Serializable]
    class TimedSoundProfile
    {
        public SoundProfile soundProfile;
        public float timeToPlay;
    }

    [Header("Time To Play = 0: Play Forever")]
    [SerializeField] TimedSoundProfile[] elements;
    [SerializeField] bool loopSequence;

    int playing;
    float timeElapsedPlaying;
    bool doneWithSequence;

    public override void Initiate()
    {
        playing = 0;
        elements[playing].soundProfile.Initiate();
        timeElapsedPlaying = 0;
        doneWithSequence = false;
    }

    void SwapToNextProfile()
    {
        elements[playing].soundProfile.Finish();
        playing = loopSequence ? ((playing + 1) % elements.Length) : playing + 1;
        if (playing == elements.Length)
        {
            doneWithSequence = true;
            return;
        }
        elements[playing].soundProfile.Initiate();
        timeElapsedPlaying = 0;
    }

    public override void AdvanceTime()
    {
        if (doneWithSequence)
        {
            return;
        }
        elements[playing].soundProfile.AdvanceTime();
        timeElapsedPlaying += Time.deltaTime;
        if (timeElapsedPlaying > elements[playing].timeToPlay && elements[playing].timeToPlay != 0)
        {
            SwapToNextProfile();
        }
    }

    public override void Finish()
    {
        if (playing < elements.Length)
        {
            elements[playing].soundProfile.Finish();
        }
        doneWithSequence = true;
    }
}
