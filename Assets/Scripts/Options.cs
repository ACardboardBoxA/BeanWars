using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class Options : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle muteToggle;

    private const string VolumeKey = "Volume";
    private const string MuteKey = "Mute";

    private void Start()
    {
        LoadSettings();
    }

    private void Update()
    {
        ApplySettings();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume);
    }

    public void SetMute(bool isMuted)
    {
        AudioListener.pause = isMuted;
        PlayerPrefs.SetInt(MuteKey, isMuted ? 1 : 0);
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey(VolumeKey))
        {
            float savedVolume = PlayerPrefs.GetFloat(VolumeKey);
            volumeSlider.value = savedVolume;
            AudioListener.volume = savedVolume;
        }

        if (PlayerPrefs.HasKey(MuteKey))
        {
            bool isMuted = PlayerPrefs.GetInt(MuteKey) == 1;
            muteToggle.isOn = isMuted;
            AudioListener.pause = isMuted;
        }
    }

    private void ApplySettings()
    {

    }
}
