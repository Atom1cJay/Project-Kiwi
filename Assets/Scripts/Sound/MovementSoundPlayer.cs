using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSoundPlayer : MonoBehaviour
{
    IMoveImmutable currentMove;
    SoundProfile storedSoundProfile;

    void Start()
    {
        MoveExecuter.instance.OnMoveChanged += (oldMove, newMove) =>
        {
            currentMove = newMove;
            SwitchProfile(newMove.GetSoundProfile());
        };
    }

    void SwitchProfile(SoundProfile newProfile)
    {
        storedSoundProfile?.Finish();
        newProfile?.Initiate();
        storedSoundProfile = newProfile;
    }

    void Update()
    {
        SoundProfile soundProfileThisFrame = currentMove?.GetSoundProfile();
        if (soundProfileThisFrame != storedSoundProfile)
        {
            SwitchProfile(soundProfileThisFrame);
        }
        else
        {
            storedSoundProfile?.AdvanceTime();
        }
    }
}
