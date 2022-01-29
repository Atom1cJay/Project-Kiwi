using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{

    [SerializeField] bool sfx, music;
    [SerializeField] Slider slider;

    public void UpdateMultiplier()
    {

        if (sfx)
        {
            AudioMasterController.instance.SetSFX(slider.value);
        }

        if (music)
        {
            AudioMasterController.instance.SetMusic(slider.value);
        }

    }
}
