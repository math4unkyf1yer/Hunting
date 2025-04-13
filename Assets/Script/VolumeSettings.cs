using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer masterMixer;


    private void Start() {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
    }

    public void SetVolume(float value) {
        if(value < 1) 
            value = .001f;
        
        RefreshSlider(value);
        PlayerPrefs.SetFloat("SavedMasterVolume", value);

        // Need to figure out a way to save volume settings to a save file later
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(value / 100) * 20f);
    }

    public void SetVolumeFromSlider() {
        SetVolume(soundSlider.value);
    }

    public void RefreshSlider(float value) {
        soundSlider.value = value;
    }
}
