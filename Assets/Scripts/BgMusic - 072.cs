using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BgMusic072 : MonoBehaviour
{
    [SerializeField] AudioSource backgroundMusic072;

    private static BgMusic072 instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        backgroundMusic072.loop = true;
        backgroundMusic072.Play();
        backgroundMusic072.volume = 0.4f;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(this.gameObject);
        }
    }
}