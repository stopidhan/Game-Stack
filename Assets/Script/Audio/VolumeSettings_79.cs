using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting_79 : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    private const float DEFAULT_VOLUME = 0.75f;

    void Start()
    {
        InitializeVolumes();
    }

    private void InitializeVolumes()
    {
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", DEFAULT_VOLUME);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", DEFAULT_VOLUME);
        
        musicSlider.value = musicVolume;
        SFXSlider.value = sfxVolume;
        
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        myMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}