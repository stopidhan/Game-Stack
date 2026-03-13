using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_79 : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource SFXSource;


    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip win;
    public AudioClip death;
    public AudioClip point;
    public AudioClip click;
    public AudioClip countdown;
    public AudioClip powerUp;
    public AudioClip damaged;

    void Start()
    {
        backgroundMusic.clip = background;
        backgroundMusic.loop = true;
        backgroundMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
