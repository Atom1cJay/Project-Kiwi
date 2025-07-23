using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound Profile/Surface")]
public class SurfaceSoundProfile : SoundProfile
{
    [SerializeField] SoundProfile boopingWaterSoundProfile;
    [SerializeField] SoundProfile notBoopingWaterSoundProfile;
    SoundProfile current;

    void UpdateSoundProfile(bool justInitiated)
    {
        SoundProfile stored = current;
        current = SurfaceManager.IsBoopingWater() ? boopingWaterSoundProfile : notBoopingWaterSoundProfile;
        if (justInitiated || stored != current)
        {
            stored?.Finish();
            current?.Initiate();
        }
    }

    public override void Initiate()
    {
        UpdateSoundProfile(true);
    }

    public override void AdvanceTime()
    {
        current?.AdvanceTime();
        UpdateSoundProfile(false);
    }

    public override void Finish()
    {
        current?.Finish();
    }
}
