using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _masterSlider, _musicSlider, _sfxSlider;

    private void Start()
    {
        _masterSlider.value = PlayerPrefs.HasKey("MasterVolValue") ? PlayerPrefs.GetFloat("MasterVolValue") : _masterSlider.maxValue;
        _musicSlider.value = PlayerPrefs.HasKey("MusicVolValue") ? PlayerPrefs.GetFloat("MusicVolValue") : _musicSlider.maxValue;
        _sfxSlider.value = PlayerPrefs.HasKey("SFXVolValue") ? PlayerPrefs.GetFloat("SFXVolValue") : _sfxSlider.maxValue;
    }

    public void SetMasterVolumeLevel(float value)
    {
        _mixer.SetFloat("MasterVol", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MasterVolValue", value);
    }
    public void SetMusicVolumeLevel(float value)
    {
        _mixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolValue", value);
    }
    public void SetSFXVolumeLevel(float value)
    {
        _mixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolValue", value);
    }
}
