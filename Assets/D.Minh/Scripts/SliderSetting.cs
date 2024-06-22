using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : MonoBehaviour
{
    Text valueText;
    [SerializeField] Slider musicSlider;

    public void SetMusicVolumeSlider()
    {
        MAudioManager.instance.SetMusicVolume(musicSlider.value); 
    }
    public void SetSFXVolumeSlider()
    {
        MAudioManager.instance.SetSFXVolume(musicSlider.value);
    }
}
