using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;
    private AudioSource audioSource;

    [SerializeField]
    private float defaultVolume = 0.5f;

    [SerializeField]
    private AudioClip musicClip; // Add this field

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Setup audio source
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Ensure audio clip is assigned
            if (musicClip != null)
            {
                audioSource.clip = musicClip;
            }
            else
            {
                Debug.LogError("No audio clip assigned to BackgroundMusic!");
            }

            audioSource.loop = true;
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", defaultVolume); // Load saved volume

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    public float GetVolume()
    {
        return audioSource != null ? audioSource.volume : 0f;
    }

}