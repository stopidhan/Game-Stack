using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    void Start()
    {
        if (volumeSlider == null)
        {
            Debug.LogError("Volume slider reference is missing!");
            return;
        }

        if (BackgroundMusic.instance == null)
        {
            Debug.LogError("BackgroundMusic instance is missing!");
            return;
        }

        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        volumeSlider.value = savedVolume;
        BackgroundMusic.instance.SetVolume(savedVolume);
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
    }

    private void UpdateVolume(float volume)
    {
        // Update volume and save the value
        BackgroundMusic.instance.SetVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}
